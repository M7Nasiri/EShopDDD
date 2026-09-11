using _01.Domain.Exceptions;
using EShop.Shared.Domain;

namespace _01.Domain.ValueObjects
{
    public sealed record Description : ValueObject
    {
        public string Value { get; }

        public Description(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new EShopDescriptionException();

            Value = value.Trim();
        }

        public static explicit operator string(Description description)
        {
            ArgumentNullException.ThrowIfNull(description);
            return description.Value;
        }

        public static explicit operator Description(string value) =>
            new(value);
    }
}
