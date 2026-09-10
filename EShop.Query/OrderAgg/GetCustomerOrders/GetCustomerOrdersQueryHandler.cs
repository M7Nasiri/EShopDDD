using Dapper;
using EShop.Infrastructure.Persistent.Dapper;
using EShop.Query.OrderAgg.DTOs;
using EShop.Shared.Query;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.OrderAgg.GetCustomerOrders
{
    public class GetCustomerOrdersQueryHandler(DapperContext dapperContext) : IQueryHandler<GetCustomerOrdersQuery, CustomerOrdersFilterResult>
    {
        public async Task<CustomerOrdersFilterResult> Handle(GetCustomerOrdersQuery request, CancellationToken cancellationToken)
        {
            var filter = request.FilterParams;
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("CustomerId", filter.CustomerId);

            var condition = "WHERE o.CustomerId = @CustomerId AND o.IsDelete = 0";

            if (filter.OrderStatus.HasValue)
            {
                condition += " AND o.Status = @Status";
                dynamicParams.Add("Status", (int)filter.OrderStatus.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                condition += @"AND EXISTS (SELECT 1 FROM OrderItems oi
                                INNER JOIN Products p ON oi.ProductId = p.Id
                                WHERE oi.OrderId = o.Id AND p.Name LIKE @Search)";
                dynamicParams.Add("Search", $"%{filter.Search.Trim()}%");
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
                        o.Id AS OrderNumber,
                        o.CreationDate,
                        o.Status AS OrderStatus,
                        ROUND(
                            (itemsSum.SubTotal * (1.0 - COALESCE(o.MembershipDiscountPercent, o.CouponPercent, 0) / 100.0)) 
                            + (CASE WHEN o.MembershipFreeShipping = 1 THEN 0 ELSE o.BaseShippingCost END)
                        , 2) AS TotalPaidAmount
                    FROM Orders o
                    CROSS APPLY (
                        SELECT ISNULL(SUM(oi.UnitPrice * oi.Quantity), 0) AS SubTotal
                        FROM OrderItems oi
                        WHERE oi.OrderId = o.Id
                    ) itemsSum
                    {condition}
                    ORDER BY o.CreatedAt DESC
                    OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY
                )
                SELECT 
                    po.Id,
                    po.CreatedAt,
                    po.OrderStatus,
                    po.TotalPaidAmount,
                    oi.ProductId,
                    oi.UnitPrice AS PurchasedPrice,
                    oi.Quantity As Count,
                    p.Name AS ProductName,
                    p.ImageName AS ProductMainImage
                FROM PagedOrders po
                INNER JOIN OrderItems oi ON po.Id = oi.OrderId
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
            result.Data = orderDictionary.Values.ToList();
            return result;
        }
    }
}
