using _01.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Entities.ValueObjects
{
    public record Coupon
    {
        public int Value { get; }

        public Coupon(int value)
        {
            if (value > 100 || value < 0)
                throw new EShopCouponException();

            Value = value;
        }
        public static implicit operator int(Coupon coupon) => coupon.Value;
        public static implicit operator Coupon(int coupon) => new(coupon);
    }
}
