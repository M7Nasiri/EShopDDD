using _01.Domain.ValueObjects;
using EShop.Shared.Abstractions.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.DoamainEvents.Orders
{
    public sealed record OrderCancelledDomainEvent(
    Id OrderId,
    Id CustomerId) : IDomainEvent
    {
        public DateTime OccurredOnUtc =>
            DateTime.UtcNow;
    }
}
