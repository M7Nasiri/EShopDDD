using EShop.Query.CouponAgg.DTOs;
using EShop.Shared.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.CouponAgg.GetCouponByCode
{
    public sealed record GetCouponByCodeQuery(string Code) : IQuery<CouponDetailsDto?>;
}
