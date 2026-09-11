using Dapper;
using EShop.Infrastructure.Persistent.Dapper;
using EShop.Query.MembershipAgg.DTOs;
using EShop.Shared.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.MembershipAgg.GetMembershipForAdmin
{
    public class GetMembershipsForAdminQueryHandler(DapperContext context)
        : IQueryHandler<GetMembershipsForAdminQuery, MembershipFilterResult>
    {
        public async Task<MembershipFilterResult> Handle(
            GetMembershipsForAdminQuery request,
            CancellationToken cancellationToken)
        {
            var filter = request.FilterParams;
            var dynamicParams = new DynamicParameters();
            var whereConditions = new StringBuilder("WHERE 1 = 1");

            if (filter.CustomerId.HasValue)
            {
                whereConditions.Append(" AND m.CustomerId = @CustomerId");
                dynamicParams.Add("CustomerId", filter.CustomerId.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.Status))
            {
                whereConditions.Append(" AND m.Status = @Status");
                dynamicParams.Add("Status", filter.Status.Trim());
            }

            if (filter.JustActive == true)
            {
                whereConditions.Append(" AND m.Status = 'Active' AND @Now >= m.StartDate AND @Now < m.EndDate");
                dynamicParams.Add("Now", DateTime.UtcNow);
            }

            if (!string.IsNullOrWhiteSpace(filter.CustomerSearch))
            {
                whereConditions.Append(@" AND (
                c.FullName LIKE @CustomerSearch OR 
                c.PhoneNumber LIKE @CustomerSearch
            )");
                dynamicParams.Add("CustomerSearch", $"%{filter.CustomerSearch.Trim()}%");
            }

            var countSql = $@"
            SELECT COUNT(1)
            FROM Memberships m
            INNER JOIN Customers c ON m.CustomerId = c.Id
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
                m.Id,
                m.CustomerId,
                c.FullName AS CustomerFullName,
                c.PhoneNumber AS CustomerPhoneNumber,
                m.PlanId,
                m.PlanTitle_Value AS PlanTitle,
                m.DiscountPercent,
                m.FreeShipping,
                m.StartDate,
                m.EndDate,
                m.Status
            FROM Memberships m
            INNER JOIN Customers c ON m.CustomerId = c.Id
            {whereConditions}
            ORDER BY m.StartDate DESC
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;";

            var items = (await connection.QueryAsync<MembershipAdminDto>(
                new CommandDefinition(querySql, dynamicParams, cancellationToken: cancellationToken)
            )).AsList();

            var result = new MembershipFilterResult
            {
                Data = items
            };
            result.GeneratePaging(totalCount, take, pageId);
            return result;
        }
    }
}
