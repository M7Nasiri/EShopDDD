using _01.Domain.Exceptions;

namespace _01.Domain.ValueObjects
{
    public sealed record ProductAttribute
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
