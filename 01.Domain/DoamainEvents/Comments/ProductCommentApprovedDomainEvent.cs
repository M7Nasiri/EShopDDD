using _01.Domain.ValueObjects;
using EShop.Shared.Domain;

namespace _01.Domain.DoamainEvents.Comment
{
   
    public sealed class ProductCommentApprovedDomainEvent : BaseDomainEvent
    {
        public Guid CommentId { get; private set; }
        public Guid ProductId { get; private set; }

        public ProductCommentApprovedDomainEvent(Guid commentId, Guid productId)
        {
            CommentId = commentId;
            ProductId = productId;
        }


    }
}
