namespace EShop.Query.ProductAgg.DTOs.Details;

public class ProductDetailsDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Stock { get; set; }
    public Guid CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public string? MainImage { get; set; }
    public List<ProductImageDto> Images { get; set; } = new();
    public List<ProductAttributeDto> Attributes { get; set; } = new();
}