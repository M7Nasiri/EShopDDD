using _01.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.ValueObjects
{
    public sealed record Quantity
    {
        public int Value { get; }

        public Quantity(int value)
        {
            if (value < 0)
                throw new EShopCountException();

            Value = value;
        }

        public static Quantity Zero => new(0);

        public Quantity Increase(int amount)
        {
            if (amount < 0)
                throw new EShopDomainException(
                    "Increase amount cannot be negative.");

            return new Quantity(Value + amount);
        }

        public Quantity Decrease(int amount)
        {
            if (amount < 0)
                throw new EShopDomainException(
                    "Decrease amount cannot be negative.");

            if (Value - amount < 0)
                throw new EShopCountException();

            return new Quantity(Value - amount);
        }
    }

}
