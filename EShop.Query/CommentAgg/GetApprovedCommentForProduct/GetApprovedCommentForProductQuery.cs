using System;
using System.Collections.Generic;
using System.Text;
using EShop.Query.CommentAgg.DTOs.Product;
using EShop.Shared.Query;

namespace EShop.Query.CommentAgg.GetApprovedCommentForProduct
{
    public sealed record GetApprovedCommentForProductQuery(Guid ProductId) : 
        IQuery<IReadOnlyList<ProductCommentSummaryDto?>>;

}
