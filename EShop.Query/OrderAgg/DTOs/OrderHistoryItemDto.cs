namespace EShop.Query.OrderAgg.DTOs;

public sealed record OrderHistoryItemDto
{
    public Guid ProductId { get; init; }
    public string? ProductName { get; init; } 
    public decimal PurchasedPrice { get; init; }         
    public int Count { get; init; }
    public string? ProductMainImage { get; init; }        
    public decimal TotalPrice => PurchasedPrice * Count;
}