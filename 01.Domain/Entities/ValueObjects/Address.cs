using _01.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Entities.ValueObjects
{
    public record Address
    {
        public string Name { get; }
        public string Value { get; }

        public Address(string name,string value)
        {
            if(string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(value))
            {
                throw new EShopAddressException(); 
            }
            Name = name;
            Value = value;
        }
        public static implicit operator string(Address address)
        {
            if (address is null) return string.Empty;
            return $"{address.Name}: {address.Value}";
        }


        public static implicit operator Address(string addressText)
        {
            if (string.IsNullOrWhiteSpace(addressText))
            {
                throw new EShopAddressException("Input string for address conversion cannot be null or empty.");
            }

            var parts = addressText.Split(':', 2);
            if (parts.Length < 2)
            {
                return new Address("Default", addressText.Trim());
            }

            return new Address(parts[0].Trim(), parts[1].Trim());
        }
    }
}
