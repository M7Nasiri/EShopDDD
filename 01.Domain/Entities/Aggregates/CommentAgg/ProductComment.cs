using _01.Domain.Consts;
using _01.Domain.DoamainEvents.Comment;
using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Entities.Aggregates.CommentAgg
{
    public sealed class ProductComment : AggregateRoot
    {
        public Guid Id { get; private set; }

        public Guid ProductId { get; private set; }

        public Guid CustomerId { get; private set; }

        public Guid? ModeratedByUserId { get; private set; }

        public string Text { get; private set; }

        public ProductCommentStatus Status { get; private set; }

        private ProductComment()
        {
        }

        public ProductComment(
            string text,
            Guid productId,
            Guid customerId)
        {
            ArgumentNullException.ThrowIfNull(productId);
            ArgumentNullException.ThrowIfNull(customerId);

            if (string.IsNullOrWhiteSpace(text))
                throw new EShopDomainException(
                    "Comment text is required.");

            Id = Guid.New();
            Text = text.Trim();
            ProductId = productId;
            CustomerId = customerId;
            Status = ProductCommentStatus.Pending;
        }
        public bool IsOwnedBy(Guid customerId)
        {
            ArgumentNullException.ThrowIfNull(customerId);

            return CustomerId == customerId;
        }

        public void EditText(string text)
        {
            if (Status == ProductCommentStatus.Approved)
            {
                throw new EShopDomainException(
                    "Approved comment cannot be edited.");
            }

            if (Status == ProductCommentStatus.Rejected)
            {
                throw new EShopDomainException(
                    "Rejected comment cannot be edited.");
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new EShopDomainException(
                    "Comment text is required.");
            }

            Text = text.Trim();
        }

        public void Approve(Guid userId)
        {
            ArgumentNullException.ThrowIfNull(userId);

            if (Status == ProductCommentStatus.Approved)
                return;

            if (Status == ProductCommentStatus.Rejected)
                throw new EShopDomainException(
                    "Rejected comment cannot be approved.");

            ModeratedByUserId = userId;
            Status = ProductCommentStatus.Approved;

            AddDomainEvent(
                new ProductCommentApprovedDomainEvent(
                    Id,
                    ProductId));
        }

        public void Reject(Guid userId)
        {
            ArgumentNullException.ThrowIfNull(userId);

            if (Status == ProductCommentStatus.Rejected)
                return;

            if (Status == ProductCommentStatus.Approved)
                throw new EShopDomainException(
                    "Approved comment cannot be rejected.");

            ModeratedByUserId = userId;
            Status = ProductCommentStatus.Rejected;
        }

    }
}
