using _01.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Entities.ValueObjects
{
    public record Count
    {
        public int Value { get; }

        public Count(int count)
        {
            if (count < 1)
                throw new EShopCountException();

            Value = count;
        }
        public static implicit operator int(Count count) => count.Value;
        public static implicit operator Count(int count) => new(count);
    }
}
