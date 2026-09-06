using _01.Domain.ValueObjects;
using EShop.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.DomainEvents.Payment
{
    public sealed class PaymentSuccededDomainEvent(
     Guid paymentId,
     Guid orderId,
     Money amount) : BaseDomainEvent
    {
        public Guid PaymentId { get; private set; } = paymentId;
        public Guid OrderId { get; private set; } = orderId;
        public Money Amount { get; private set; } = amount;
    }
}
