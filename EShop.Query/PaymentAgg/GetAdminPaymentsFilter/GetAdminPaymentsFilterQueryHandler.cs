using Dapper;
using EShop.Infrastructure.Persistent.Dapper;
using EShop.Query.OrderAgg.DTOs;
using EShop.Query.OrderAgg.GetCustomersOrdersForAdmin;
using EShop.Query.PaymentAgg.DTOs.Admin;
using EShop.Shared.Application;
using EShop.Shared.Query;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace EShop.Query.PaymentAgg.GetAdminPaymentsFilter
{
    public class GetAdminPaymentsFilterQueryHandler(DapperContext context):
        IQueryHandler<GetAdminPaymentsFilterQuery, AdminPaymentFilterResult>
    {
        public async Task<AdminPaymentFilterResult> Handle(GetAdminPaymentsFilterQuery request, CancellationToken cancellationToken)
        {
            using var connection = context.CreateConnection();
            var whereClause = new StringBuilder(" WHERE 1=1 ");
            var parameters = new DynamicParameters();

            var filter = request.FilterParams;

            if (filter.Status.HasValue)
            {
                whereClause.Append(" AND p.Status = @Status ");
                parameters.Add("Status", filter.Status.Value.ToString());
            }

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                whereClause.Append(" AND (o.ShippingReceiverName LIKE @Search OR CAST(o.Id AS NVARCHAR(50)) = @ExactSearch) ");
                parameters.Add("Search", $"%{filter.Search}%");
                parameters.Add("ExactSearch", filter.Search);
            }

            var countSql = $"SELECT COUNT(1) FROM Payments p INNER JOIN Orders o ON p.OrderId = o.Id {whereClause}";
            var totalCount = await connection.ExecuteScalarAsync<int>(countSql, parameters);

            var skip = (filter.PageId - 1) * filter.Take;
            parameters.Add("Skip", skip);
            parameters.Add("Take", filter.Take);


            var sql = $@"
            SELECT p.Id , p.OrderId, c.FullName AS CustomerFullName, 
                   p.Amount, p.Method, p.Status, p.CreationDate, p.GatewayTransactionId , p.RefundTransactionId
            FROM Payments p
            INNER JOIN Orders o ON p.OrderId = o.Id
            INNER JOIN Customers c ON o.CustomerId = c.Id
            {whereClause}
            ORDER BY p.CreationDate DESC
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY";

            var payments = (await connection.QueryAsync<AdminPaymentListItemDto>(sql, parameters)).ToList();

            var result = new AdminPaymentFilterResult()
            {
                Data = payments
            };
            result.GeneratePaging(totalCount, filter.Take, filter.PageId);
            return result;
        }
    }
}
