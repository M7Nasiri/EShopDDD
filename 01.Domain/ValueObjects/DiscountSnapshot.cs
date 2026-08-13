using _01.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.ValueObjects
{
    public sealed record DiscountSnapshot
    {
        public string Reason { get; }
        public int Percent { get; }

        public DiscountSnapshot(
            string reason,
            int percent)
        {
            if (string.IsNullOrWhiteSpace(reason))
                throw new EShopDomainException(
                    "Discount reason is required.");

            if (percent is < 1 or > 100)
                throw new EShopDomainException(
                    "Discount percent must be between 1 and 100.");

            Reason = reason.Trim();
            Percent = percent;
        }

        public Money Apply(Money price) =>
            price.ApplyPercentageDiscount(Percent);
    }
}
