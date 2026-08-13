using _01.Domain.ValueObjects;
using EShop.Shared.Abstractions.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.DoamainEvents.Payment
{
    public sealed record PaymentSucceededDomainEvent(
     Id PaymentId,
     Id OrderId,
     Money Amount) : IDomainEvent
    {
        public DateTime OccurredOnUtc =>
            DateTime.UtcNow;
    }
}
