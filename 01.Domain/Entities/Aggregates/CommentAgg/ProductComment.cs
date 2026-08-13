using _01.Domain.Consts;
using _01.Domain.DoamainEvents.Comment;
using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Abstractions.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Entities.Aggregates.CommentAgg
{
    public sealed class ProductComment : AggregateRoot
    {
        public Id Id { get; private set; }

        public Id ProductId { get; private set; }

        public Id CustomerId { get; private set; }

        public Id? ModeratedByUserId { get; private set; }

        public string Text { get; private set; }

        public ProductCommentStatus Status { get; private set; }

        private ProductComment()
        {
        }

        public ProductComment(
            Id id,
            string text,
            Id productId,
            Id customerId)
        {
            ArgumentNullException.ThrowIfNull(id);
            ArgumentNullException.ThrowIfNull(productId);
            ArgumentNullException.ThrowIfNull(customerId);

            if (string.IsNullOrWhiteSpace(text))
                throw new EShopDomainException(
                    "Comment text is required.");

            Id = id;
            Text = text.Trim();
            ProductId = productId;
            CustomerId = customerId;
            Status = ProductCommentStatus.Pending;
        }

        public void EditText(string text)
        {
            if (Status == ProductCommentStatus.Approved)
                throw new EShopDomainException(
                    "Approved comment cannot be edited.");

            if (string.IsNullOrWhiteSpace(text))
                throw new EShopDomainException(
                    "Comment text is required.");

            Text = text.Trim();
        }

        public void Approve(Id userId)
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

        public void Reject(Id userId)
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
