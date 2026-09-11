using _01.Domain.Consts;
using EShop.Query.OrderAgg.DTOs.GetCustomerOrderDetails;
using EShop.Shared.Application;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using _01.Domain.Exceptions;
using Dapper;
using EShop.Infrastructure.Persistent.Dapper;
using EShop.Shared.Query;

namespace EShop.Query.OrderAgg.GetCustomerOrderDetails
{
    public class GetCustomerOrderDetailsQueryHandler(DapperContext context)
        : IQueryHandler<GetCustomerOrderDetailsQuery, CustomerOrderDetailDto?>
    {
        public async Task<CustomerOrderDetailDto?> Handle(
            GetCustomerOrderDetailsQuery request,
            CancellationToken cancellationToken)
        {
            using var connection = context.CreateConnection();

            const string sql = @"

            SELECT 
                o.Id AS OrderId,
                o.Status,
                o.CreatedAt,
                o.BaseShippingCost,
                o.CouponCode,
                o.CouponPercent,
                o.MembershipDiscountTitle,
                o.MembershipDiscountPercent,
                ISNULL(o.MembershipFreeShipping, 0) AS FreeShipping,
                o.ShippingReceiverName AS ReceiverName,
                o.ShippingPhoneNumber AS PhoneNumber,
                o.ShippingProvince AS Province,
                o.ShippingCity AS City,
                o.ShippingStreet AS Street,
                o.ShippingPlaque AS Plaque,
                o.ShippingPostalCode AS PostalCode
            FROM Orders o
            WHERE o.Id = @OrderId AND o.CustomerId = @CustomerId;


            SELECT 
                oi.ProductId,
                p.Title AS ProductTitle,
                p.ImageName AS ProductImageName,
                oi.Quantity,
                oi.UnitPrice,
                (oi.UnitPrice * oi.Quantity) AS TotalPrice
            FROM OrderItems oi
            INNER JOIN Products p ON p.Id = oi.ProductId
            WHERE oi.OrderId = @OrderId;";

            await using var multi = await connection.QueryMultipleAsync(sql, new
            {
                request.OrderId,
                request.CustomerId
            });

            var orderHeader = await multi.ReadFirstOrDefaultAsync<dynamic>();
            if (orderHeader is null)
                throw new EShopDomainException("The order was not found!");

            var items = (await multi.ReadAsync<CustomerOrderItemDetailDto>()).ToList();

            var address = new CustomerShippingAddressDto(
                orderHeader.ReceiverName,
                orderHeader.PhoneNumber,
                orderHeader.Province,
                orderHeader.City,
                orderHeader.Street,
                orderHeader.Plaque,
                orderHeader.PostalCode);

            var result = new CustomerOrderDetailDto(
                orderHeader.OrderId,
                (OrderStatus)orderHeader.Status,
                orderHeader.CreatedAt,
                orderHeader.BaseShippingCost,
                orderHeader.CouponCode,
                orderHeader.CouponPercent,
                orderHeader.MembershipDiscountTitle,
                orderHeader.MembershipDiscountPercent,
                orderHeader.FreeShipping,
                address,
                items);

            return result;
        }
    }
}
