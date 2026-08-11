using _01.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Entities.ValueObjects
{
    public class Id
    {
        public Guid Value { get; }

        public Id(Guid value)
        {
            if (value == Guid.Empty)
                throw new EShopIdException();
            Value = value;
        }
        public static implicit operator Guid(Id id) => id.Value;
        public static implicit operator Id(Guid id) => new(id);

    }
}
