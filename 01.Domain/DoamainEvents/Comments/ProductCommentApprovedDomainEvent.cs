using _01.Domain.ValueObjects;
using EShop.Shared.Abstractions.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.DoamainEvents.Comment
{
    public sealed record ProductCommentApprovedDomainEvent(
    Id CommentId,
    Id ProductId) : IDomainEvent
    {
        public DateTime OccurredOnUtc =>
            DateTime.UtcNow;
    }
}
