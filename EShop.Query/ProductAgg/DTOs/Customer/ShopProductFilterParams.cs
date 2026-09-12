using EShop.Shared.Query.Filter;

namespace EShop.Query.ProductAgg.DTOs.Customer;

public class ShopProductFilterParams : BaseFilterParam
{
    public string? Search { get; set; }
    public Guid? CategoryId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public bool OnlyAvailable { get; set; } = false;
    public ProductSortBy SortBy { get; set; } = ProductSortBy.Latest;
}