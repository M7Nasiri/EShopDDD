using EShop.Shared.Query;

namespace EShop.Query.CustomerAgg.DTOs.Orders;

public sealed class CustomerOrderSummaryDto : BaseDto
{
    public long OrderNumber { get; init; }               
    public string OrderStatus { get; init; } = default!;
    public decimal TotalPaidAmount { get; init; }
    public List<OrderHistoryItemDto> Items { get; set; } = new();
}