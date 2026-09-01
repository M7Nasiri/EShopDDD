using _01.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.ValueObjects
{
    public sealed record Address
    {
        public string ReceiverName { get; }
        public string PhoneNumber { get; }
        public string Title { get; }
        public string Province { get; }
        public string City { get; }
        public string Street { get; }
        public string Plaque { get; }
        public string PostalCode { get; }

        public Address(string receiverName,
            string phoneNumber,
            string title,
            string province,
            string city,
            string street,
            string plaque,
            string postalCode)
        {
            if (string.IsNullOrWhiteSpace(receiverName))
                throw new EShopDomainException("نام تحویل‌گیرنده الزامی است.");

            if (string.IsNullOrWhiteSpace(phoneNumber) || phoneNumber.Trim().Length != 11)
                throw new EShopDomainException("شماره تماس باید ۱۱ رقمی باشد.");
            if (string.IsNullOrWhiteSpace(title))
                throw new EShopDomainException("عنوان آدرس الزامی است.");

            if (string.IsNullOrWhiteSpace(province))
                throw new EShopDomainException("استان الزامی است.");

            if (string.IsNullOrWhiteSpace(city))
                throw new EShopDomainException("شهر الزامی است.");

            if (string.IsNullOrWhiteSpace(street))
                throw new EShopDomainException("خیابان الزامی است.");

            if (string.IsNullOrWhiteSpace(plaque))
                throw new EShopDomainException("پلاک الزامی است.");

            if (string.IsNullOrWhiteSpace(postalCode) || postalCode.Trim().Length != 10)
                throw new EShopDomainException("کد پستی باید ۱۰ رقمی باشد.");

            Title = title.Trim();
            Province = province.Trim();
            City = city.Trim();
            Street = street.Trim();
            Plaque = plaque.Trim();
            PostalCode = postalCode.Trim();
            ReceiverName = receiverName.Trim();
            PhoneNumber = phoneNumber.Trim();
        }

        public override string ToString() =>
            $"{Title}: {Province}، {City}، {Street}، پلاک {Plaque}، کد پستی: {PostalCode}";
    }
}
