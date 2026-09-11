using Dapper;
using EShop.Infrastructure.Persistent.Dapper;
using EShop.Query.MembershipPlanAgg.DTOs.Admin;
using EShop.Shared.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.MembershipPlanAgg.GetAllPlansForAdmin
{
    public class GetAllPlansForAdminQueryHandler(DapperContext context) :
        IQueryHandler<GetAllPlansForAdminQuery , MembershipPlanFilterResult>
    {
        public async Task<MembershipPlanFilterResult> Handle(GetAllPlansForAdminQuery request, CancellationToken cancellationToken)
        {
            var filter = request.filterParams;
            var dynamicParams = new DynamicParameters();
            var whereConditions = new StringBuilder("WHERE 1 = 1");

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                whereConditions.Append(" AND Name LIKE @Search");
                dynamicParams.Add("Search", $"%{filter.Search.Trim()}%");
            }

            if (filter.IsActive.HasValue)
            {
                whereConditions.Append(" AND IsActive = @IsActive");
                dynamicParams.Add("IsActive", filter.IsActive.Value);
            }

            var countSql = $@"
            SELECT COUNT(1)
            FROM MembershipPlans
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
                Id,
                Name,
                Price,
                DiscountPercent,
                FreeShipping,
                DurationInDays,
                IsActive
            FROM MembershipPlans
            {whereConditions}
            ORDER BY CreationDate ASC
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;";

            var items = (await connection.QueryAsync<MembershipPlanAdminDto>(
                new CommandDefinition(querySql, dynamicParams, cancellationToken: cancellationToken)
            )).AsList();

            var result = new MembershipPlanFilterResult
            {
                Data = items
            };
            result.GeneratePaging(totalCount, take, pageId);
            return result;
        }
    }
}
