using _01.Domain.Consts;
using EShop.Shared.Query;

namespace EShop.Query.CommentAgg.DTOs.Product;

public sealed class ProductCommentSummaryDto : BaseDto
{
    public Guid ProductId { get; init; }
    public Guid CustomerId { get; init; }
    public string CustomerFullName { get; init; } 
    public string Text { get; init; }
    //public  ProductCommentStatus Status { get; set; }

}