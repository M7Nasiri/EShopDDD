using EShop.Shared.Query.Filter;

namespace EShop.Query.CartAgg.DTOs;

public class CartFilterParam : BaseFilterParam
{
    public Guid? CustomerId { get; set; }
    public string? GuestId { get; set; }
    public bool? HasItemsOnly { get; set; } 
}