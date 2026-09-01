using _01.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.ValueObjects
{
    public sealed record AppliedCouponSnapshot
    {
        public Guid CouponId { get; }
        public string Code { get; }
        public int Percent { get; }

        public AppliedCouponSnapshot(
            string code,
            int percent)
        {
            CouponId = Guid.New();

            if (string.IsNullOrWhiteSpace(code))
                throw new EShopCouponException();

            if (percent is < 1 or > 100)
                throw new EShopCouponException();

            Code = code.Trim();
            Percent = percent;
        }
    }
}
