using EShop.Shared.Query.Filter;

namespace EShop.Query.ProductAgg.DTOs.Admin;

public class ProductAdminFilterParams : BaseFilterParam
{
    public string? Search { get; set; }
    public Guid? CategoryId { get; set; }
    public bool? OnlyInStock { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
}