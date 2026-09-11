using _01.Domain.Consts;
using EShop.Query.OrderAgg.DTOs.GetAdminOrderDetails;
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

namespace EShop.Query.OrderAgg.GetAdminOrderDetails
{
    public class GetAdminOrderDetailsQueryHandler(DapperContext context)
        : IQueryHandler<GetAdminOrderDetailsQuery, AdminOrderDetailDto?>
    {
        public async Task<AdminOrderDetailDto?> Handle(
            GetAdminOrderDetailsQuery request,
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


                c.Id AS CustomerId,
                c.FullName AS CustomerFullName,
                c.PhoneNumber AS CustomerPhoneNumber,
                c.Email AS CustomerEmail,


                o.ShippingReceiverName AS ReceiverName,
                o.ShippingPhoneNumber AS ShippingPhoneNumber,
                o.ShippingProvince AS Province,
                o.ShippingCity AS City,
                o.ShippingStreet AS Street,
                o.ShippingPlaque AS Plaque,
                o.ShippingPostalCode AS PostalCode
            FROM Orders o
            INNER JOIN Customers c ON c.Id = o.CustomerId
            WHERE o.Id = @OrderId;

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

            using var multi = await connection.QueryMultipleAsync(sql, new { request.OrderId });

            var header = await multi.ReadFirstOrDefaultAsync<dynamic>();
            if (header is null)
                throw new EShopDomainException("سفارش مورد نظر یافت نشد.");

            var items = (await multi.ReadAsync<CustomerOrderItemDetailDto>()).ToList();

            var customerInfo = new AdminCustomerInfoDto(
                header.CustomerId,
                header.CustomerFullName,
                header.CustomerPhoneNumber,
                header.CustomerEmail);

            var address = new CustomerShippingAddressDto(
                header.ReceiverName,
                header.ShippingPhoneNumber,
                header.Province,
                header.City,
                header.Street,
                header.Plaque,
                header.PostalCode);

            var result = new AdminOrderDetailDto(
                header.OrderId,
                (OrderStatus)header.Status,
                header.CreatedAt,
                header.BaseShippingCost,
                header.CouponCode,
                header.CouponPercent,
                header.MembershipDiscountTitle,
                header.MembershipDiscountPercent,
                header.FreeShipping,
                customerInfo,
                address,
                items);

            return result;
        }
    }
}
