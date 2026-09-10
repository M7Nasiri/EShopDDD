using EShop.Shared.Query;

namespace EShop.Query.CommentAgg.DTOs.Admin;

public sealed class CommentAdminSummaryDto : BaseDto
{
    public Guid ProductId { get; init; }
    public string ProductName { get; init; } 
    public string? ProductImage { get; init; }
    public Guid CustomerId { get; init; }
    public string CustomerFullName { get; init; } 
    public string Text { get; init; }
    public string Status { get; init; } 

}