using _01.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.ValueObjects
{
    public sealed record ShippingAddress
    {
        public string RecipientName { get; }
        public string Province { get; }
        public string City { get; }
        public string Address { get; }
        public string PostalCode { get; }
        public string PhoneNumber { get; }

        public ShippingAddress(
            string recipientName,
            string province,
            string city,
            string address,
            string postalCode,
            string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(recipientName))
                throw new EShopAddressException();

            if (string.IsNullOrWhiteSpace(province))
                throw new EShopAddressException();

            if (string.IsNullOrWhiteSpace(city))
                throw new EShopAddressException();

            if (string.IsNullOrWhiteSpace(address))
                throw new EShopAddressException();

            if (string.IsNullOrWhiteSpace(postalCode))
                throw new EShopAddressException();

            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new EShopAddressException();

            RecipientName = recipientName.Trim();
            Province = province.Trim();
            City = city.Trim();
            Address = address.Trim();
            PostalCode = postalCode.Trim();
            PhoneNumber = phoneNumber.Trim();
        }
    }
}
