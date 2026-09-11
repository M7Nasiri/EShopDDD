using _01.Domain.Consts;

namespace EShop.Query.OrderAgg.DTOs.GetCustomerOrderDetails;

public record CustomerOrderDetailDto(
    Guid OrderId,
    OrderStatus Status,
    DateTime CreatedAt,
    decimal BaseShippingCost,
    string? CouponCode,
    int? CouponPercent,
    string? MembershipDiscountTitle,
    int? MembershipDiscountPercent,
    bool FreeShipping,
    CustomerShippingAddressDto ShippingAddress,
    List<CustomerOrderItemDetailDto> Items);