using Dapper;
using EShop.Infrastructure.Persistent.Dapper;
using EShop.Query.CustomerAgg.DTOs.Orders;
using EShop.Shared.Query;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.CustomerAgg.GetCustomerOrders
{
    public class GetCustomerOrdersQueryHandler(DapperContext dapperContext) : IQueryHandler<GetCustomerOrdersQuery, CustomerOrdersFilterResult>
    {
        public async Task<CustomerOrdersFilterResult> Handle(GetCustomerOrdersQuery request, CancellationToken cancellationToken)
        {
            var filter = request.FilterParams;
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("CustomerId", filter.CustomerId);

            var condition = "WHERE o.CustomerId = @CustomerId AND o.IsDelete = 0";

            if (!string.IsNullOrWhiteSpace(filter.OrderStatus))
            {
                condition += " AND o.Status = @Status";
                dynamicParams.Add("Status", filter.OrderStatus.Trim());
            }

            using var connection = dapperContext.CreateConnection();
     
            var countSql = $"SELECT COUNT(1) FROM Orders o {condition}";
            var totalCount = await connection.ExecuteScalarAsync<int>(
                new CommandDefinition(countSql, dynamicParams, cancellationToken: cancellationToken)
            );

            var skip = (filter.PageId - 1) * filter.Take;
            dynamicParams.Add("Skip", skip);
            dynamicParams.Add("Take", filter.Take);

            var sql = $@"
                -- سفارش‌های صفحه جاری
                WITH PagedOrders AS (
                    SELECT 
                        o.Id,
                        o.CreationDate,
                        o.Status AS OrderStatus,
                        o.TotalPaidAmount
                    FROM Orders o
                    {condition}
                    ORDER BY o.CreationDate DESC
                    OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY
                )
                SELECT 
                    po.Id,
                    po.CreationDate,
                    po.OrderStatus,
                    po.TotalPaidAmount,
                    oi.ProductId,
                    oi.UnitPrice AS PurchasedPrice,
                    oi.Count,
                    p.Name As ProductName,
                    p.ImageName AS ProductMainImage
                FROM PagedOrders po
                INNER JOIN OrderItems oi ON po.OrderId = oi.OrderId
                LEFT JOIN Products p ON oi.ProductId = p.Id;";

            var orderDictionary = new Dictionary<Guid, CustomerOrderSummaryDto>();

            await connection.QueryAsync<CustomerOrderSummaryDto, OrderHistoryItemDto, CustomerOrderSummaryDto>(
                new CommandDefinition(sql, dynamicParams, cancellationToken: cancellationToken),
                (order, item) =>
                {
                    if (!orderDictionary.TryGetValue(order.Id, out var currentOrder))
                    {
                        currentOrder = order;
                        currentOrder.Items = new List<OrderHistoryItemDto>();
                        orderDictionary.Add(currentOrder.Id, currentOrder);
                    }

                    if (item != null)
                    {
                        currentOrder.Items.Add(item);
                    }

                    return currentOrder;
                },
                splitOn: "ProductId"
            );

            var result = new CustomerOrdersFilterResult();
            result.GeneratePaging(totalCount, filter.Take, filter.PageId);
            return result;
        }
    }
}
