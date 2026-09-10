using EShop.Shared.Query;

namespace EShop.Query.CommentAgg.DTOs.Customer;

public sealed class CommentCustomerSummaryDto : BaseDto
{
    public Guid ProductId { get; init; }
    public string ProductName { get; init; } 
    public string? ProductImage { get; init; }
    public string CustomerFullName { get; init; } 
    public string Text { get; init; }
    public string Status { get; init; } 

}