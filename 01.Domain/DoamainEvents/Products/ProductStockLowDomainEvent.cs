using _01.Domain.ValueObjects;
using EShop.Shared.Abstractions.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.DoamainEvents.Products
{
    public sealed record ProductStockLowDomainEvent(
    Id ProductId,
    int CurrentStock) : IDomainEvent
    {
        public DateTime OccurredOnUtc =>
            DateTime.UtcNow;
    }
}
