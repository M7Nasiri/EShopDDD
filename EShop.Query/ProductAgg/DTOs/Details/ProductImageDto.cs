namespace EShop.Query.ProductAgg.DTOs.Details;

public class ProductImageDto
{
    public Guid Id { get; set; }
    public string ImageName { get; set; } = string.Empty;
    public int Sequence { get; set; }
}