using _01.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.ValueObjects
{
    public sealed record AppliedCouponSnapshot
    {
        public Id CouponId { get; }
        public string Code { get; }
        public int Percent { get; }

        public AppliedCouponSnapshot(
            Id couponId,
            string code,
            int percent)
        {
            ArgumentNullException.ThrowIfNull(couponId);

            if (string.IsNullOrWhiteSpace(code))
                throw new EShopCouponException();

            if (percent is < 1 or > 100)
                throw new EShopCouponException();

            CouponId = couponId;
            Code = code.Trim();
            Percent = percent;
        }
    }
}
