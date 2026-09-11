using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using EShop.Infrastructure.Persistent.Dapper;
using EShop.Query.MembershipAgg.DTOs;
using EShop.Shared.Query;

namespace EShop.Query.MembershipAgg.GetMembershipById
{
    public class GetMembershipByIdQueryHandler(DapperContext context)
        : IQueryHandler<GetMembershipByIdQuery, MembershipAdminDto?>
    {
        public async Task<MembershipAdminDto?> Handle(
            GetMembershipByIdQuery request,
            CancellationToken cancellationToken)
        {
            const string sql = @"
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
            WHERE m.Id = @Id;";

            using var connection = context.CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<MembershipAdminDto>(
                new CommandDefinition(sql, new { request.Id }, cancellationToken: cancellationToken)
            );
        }
    }
}
