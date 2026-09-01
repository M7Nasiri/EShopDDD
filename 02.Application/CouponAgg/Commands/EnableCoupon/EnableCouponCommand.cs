using EShop.Shared.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.CouponAgg.Commands.EnableCoupon
{
    public sealed record EnableCouponCommand(Guid CouponId) : IBaseCommand; 
}
