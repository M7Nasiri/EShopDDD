using EShop.Shared.Query;

namespace EShop.Query.OrderAgg.DTOs;

public sealed class CustomerOrderSummaryDto : BaseDto
{
    public string OrderStatus { get; init; } = default!;
    public decimal TotalPaidAmount { get; init; }
    public List<OrderHistoryItemDto> Items { get; set; } = new();
}