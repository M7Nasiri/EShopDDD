using EShop.Shared.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.CouponAgg.Commands.AddCoupon
{
    public sealed record AddCouponCommand(string Code, int Percent, DateTime StartDate, DateTime EndDate,
        int UsageLimit) : IBaseCommand;
}
