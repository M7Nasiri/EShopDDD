using EShop.Shared.Query;

namespace EShop.Query.ProductAgg.DTOs.Customer;

public class ShopProductItemDto : BaseDto
{
    public string Name { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Stock { get; set; }
    public string? MainImage { get; set; }
    public Guid CategoryId { get; set; }

}