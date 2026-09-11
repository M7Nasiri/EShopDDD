namespace EShop.Query.OrderAgg.DTOs.GetActiveDraftOrder;

public record DraftOrderDto(
    Guid OrderId,
    List<DraftOrderItemDto> Items,
    decimal BaseShippingCost,
    string? CouponCode,
    int? CouponPercent,
    string? MembershipDiscountTitle,
    int? MembershipDiscountPercent,
    bool? FreeShipping);