using _01.Domain.Consts;
using EShop.Query.OrderAgg.DTOs.GetCustomerOrderDetails;

namespace EShop.Query.OrderAgg.DTOs.GetAdminOrderDetails;

public record AdminOrderDetailDto(
    Guid OrderId,
    OrderStatus Status,
    DateTime CreatedAt,
    decimal BaseShippingCost,
    string? CouponCode,
    int? CouponPercent,
    string? MembershipDiscountTitle,
    int? MembershipDiscountPercent,
    bool FreeShipping,
    AdminCustomerInfoDto CustomerInfo,
    CustomerShippingAddressDto ShippingAddress,
    List<CustomerOrderItemDetailDto> Items);