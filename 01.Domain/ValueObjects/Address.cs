using _01.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.ValueObjects
{
    public sealed record Address
    {
        public string Title { get; }
        public string Value { get; }

        public Address(string title, string value)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new EShopAddressException();

            if (string.IsNullOrWhiteSpace(value))
                throw new EShopAddressException();

            Title = title.Trim();
            Value = value.Trim();
        }

        public override string ToString() =>
            $"{Title}: {Value}";
    }
}
