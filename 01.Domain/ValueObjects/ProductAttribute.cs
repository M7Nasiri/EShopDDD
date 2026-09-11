using _01.Domain.Exceptions;
using EShop.Shared.Domain;

namespace _01.Domain.ValueObjects
{
    public sealed record ProductAttribute : ValueObject
    {
        public string Key { get; }
        public string Value { get; }

        public ProductAttribute(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new EShopProductAttributeKeyException();

            if (string.IsNullOrWhiteSpace(value))
                throw new EShopProductAttributeValueException();

            Key = key.Trim();
            Value = value.Trim();
        }
    }
}
