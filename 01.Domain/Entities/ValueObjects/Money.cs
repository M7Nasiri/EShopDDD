using _01.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Entities.ValueObjects
{
    public record Money
    {
        public decimal Amount { get; }
        public Money(decimal amount)
        {
            if (amount < 0)
                throw new EShopMoneyException();
            Amount = amount;
        }
        public static Money operator *(Money price, int count) => new Money(price.Amount * count);
        public static implicit operator decimal(Money money) => money.Amount;
        public static implicit operator Money(decimal amount) => new Money(amount);

    }
}
