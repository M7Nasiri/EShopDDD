using _01.Domain.ValueObjects;
using EShop.Shared.Abstractions.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.DoamainEvents.Orders
{
    public sealed record OrderFinalizedDomainEvent(
    Id OrderId,
    Id CustomerId,
    Money TotalPrice) : IDomainEvent
    {
        public DateTime OccurredOnUtc =>
            DateTime.UtcNow;
    }
}
