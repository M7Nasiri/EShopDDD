using System.Text;
using Dapper;
using EShop.Infrastructure.Persistent.Dapper;
using EShop.Query.CouponAgg.DTOs;
using EShop.Shared.Query;

namespace EShop.Query.CouponAgg.GetCouponsForAdmin;

public class GetCouponsForAdminQueryHandler(DapperContext context)
    : IQueryHandler<GetCouponsForAdminQuery, CouponFilterResult>
{
    public async Task<CouponFilterResult> Handle(GetCouponsForAdminQuery request, CancellationToken cancellationToken)
    {
        var filter = request.FilterParams;
        var dynamicParams = new DynamicParameters();
        var whereConditions = new StringBuilder("WHERE 1 = 1");


        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            whereConditions.Append(" AND c.Code LIKE @Search");
            dynamicParams.Add("Search", $"%{filter.Search.Trim()}%");
        }


        if (filter.IsActive.HasValue)
        {
            whereConditions.Append(" AND c.IsActive = @IsActive");
            dynamicParams.Add("IsActive", filter.IsActive.Value);
        }

        if (filter.JustValid == true)
        {
            whereConditions.Append(@" 
                    AND c.IsActive = 1 
                    AND @Now >= c.StartDate 
                    AND @Now < c.EndDate 
                    AND (c.UsageLimit IS NULL OR c.UsedCount < c.UsageLimit)");
            dynamicParams.Add("Now", DateTime.UtcNow);
        }

        var countSql = $@"
                SELECT COUNT(1)
                FROM Coupons c
                {whereConditions};";

        using var connection = context.CreateConnection();

        var totalCount = await connection.ExecuteScalarAsync<int>(
            new CommandDefinition(countSql, dynamicParams, cancellationToken: cancellationToken)
        );


        var pageId = filter.PageId <= 0 ? 1 : filter.PageId;
        var take = filter.Take <= 0 ? 10 : filter.Take;
        var skip = (pageId - 1) * take;

        dynamicParams.Add("Skip", skip);
        dynamicParams.Add("Take", take);

        var querySql = $@"
                SELECT 
                    c.Id,
                    c.CreationDate,
                    c.Code,
                    c.[Percent],
                    c.CreatedByUserId,
                    COALESCE(a.FullName,  N'نامشخص') AS CreatorFullName,
                    c.StartDate,
                    c.EndDate,
                    c.IsActive,
                    c.UsageLimit,
                    c.UsedCount
                FROM Coupons c
                LEFT JOIN Admins a ON c.CreatedByUserId = a.Id
                {whereConditions}
                ORDER BY c.CreationDate DESC
                OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;";

        var items = (await connection.QueryAsync<CouponAdminSummaryDto>(
            new CommandDefinition(querySql, dynamicParams, cancellationToken: cancellationToken)
        )).AsList();

        var result = new CouponFilterResult()
        {
            Data = items
        };
        result.GeneratePaging(totalCount, take, pageId);
        
        return result;
    }
}