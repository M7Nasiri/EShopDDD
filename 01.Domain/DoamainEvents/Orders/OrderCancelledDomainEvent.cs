using _01.Domain.ValueObjects;
using EShop.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.DoamainEvents.Orders
{
    public sealed class OrderCancelledDomainEvent(
    Guid orderId,
    Guid customerId,
    IReadOnlyList<OrderCancelledItemDto> items) : BaseDomainEvent
    {
        public Guid OrderId { get; private set; } = orderId;
        public Guid CustomerId { get; private set; } = customerId;
        public IReadOnlyList<OrderCancelledItemDto> Items { get; private set; } = items;
    }

    public sealed record OrderCancelledItemDto(Guid ProductId, int Quantity);
}
