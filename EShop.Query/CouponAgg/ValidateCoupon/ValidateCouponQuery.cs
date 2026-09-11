using EShop.Query.CouponAgg.DTOs;
using EShop.Shared.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.CouponAgg.ValidateCoupon
{
    public record ValidateCouponQuery(string Code) : IQuery<ValidateCouponDto>;
}
