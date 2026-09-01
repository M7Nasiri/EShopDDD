using EShop.Shared.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.CouponAgg.Commands.DeleteCoupon
{
    public sealed record DeleteCouponCommand(Guid CouponId) : IBaseCommand
    {
    }
}
