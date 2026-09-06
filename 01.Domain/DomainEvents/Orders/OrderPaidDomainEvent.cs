using _01.Domain.ValueObjects;
using EShop.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.DomainEvents.Orders
{
    public sealed class OrderPaidDomainEvent(
    Guid orderId,
    Guid customerId,
    Money totalPrice) : BaseDomainEvent
    {
        public Guid OrderId { get; private set; } = orderId;
        public Guid CustomerId { get; private set; } = customerId;
        public Money TotalPrice { get; private set; } = totalPrice;
    }
}
