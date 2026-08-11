using _01.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Entities.ValueObjects
{
    public record Description
    {
        public string Value { get; }
        public Description(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new EShopDescriptionException();
            }
            Value = value.Trim();
        }
        public static implicit operator string(Description description) => description.Value;
        public static implicit operator Description(string description) => new(description);
    }
}
