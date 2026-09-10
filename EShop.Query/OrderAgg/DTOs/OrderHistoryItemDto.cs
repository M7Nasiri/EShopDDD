namespace EShop.Query.OrderAgg.DTOs;

public sealed record OrderHistoryItemDto
{
    public Guid ProductId { get; init; }
    public string ProductName { get; init; } = default!; // نام ثبت شده در سفارش
    public decimal PurchasedPrice { get; init; }          // قیمت زمان خرید
    public int Count { get; init; }
    public string? ProductMainImage { get; init; }        // تصویر از جدول کالا
    public decimal TotalPrice => PurchasedPrice * Count;
}