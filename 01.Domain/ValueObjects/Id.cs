using _01.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.ValueObjects
{
    public sealed record Id
    {
        public Guid Value { get; }

        public Id(Guid value)
        {
            if (value == Guid.Empty)
                throw new EShopIdException();

            Value = value;
        }

        public static Id New() => new(Guid.NewGuid());

        public static explicit operator Guid(Id id)
        {
            ArgumentNullException.ThrowIfNull(id);
            return id.Value;
        }

        public static explicit operator Id(Guid value) =>
            new(value);

        public override string ToString() =>
            Value.ToString();
    }
}
