using EShop.Query.CouponAgg.DTOs;
using EShop.Shared.Query;
using System;
using System.Collections.Generic;

namespace EShop.Query.CouponAgg.GetCouponsForAdmin
{
    public class GetCouponsForAdminQuery(CouponFilterParams FilterParams)
        : QueryFilter<CouponFilterResult, CouponFilterParams>(FilterParams);
}
