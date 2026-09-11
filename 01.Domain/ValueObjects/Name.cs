using _01.Domain.Exceptions;
using EShop.Shared.Domain;

namespace _01.Domain.ValueObjects
{
    public sealed record Name : ValueObject
    {
        public string Value { get; }

        public Name(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new EShopFullNameException();

            Value = value.Trim();
        }

        public static explicit operator string(Name name)
        {
            ArgumentNullException.ThrowIfNull(name);
            return name.Value;
        }

        public static explicit operator Name(string value) =>
            new(value);
    }
}
