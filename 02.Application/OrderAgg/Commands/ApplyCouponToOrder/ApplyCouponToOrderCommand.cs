using EShop.Shared.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.OrderAgg.Commands.ApplyCouponToOrder
{
    public sealed record ApplyCouponToOrderCommand(string CouponCode) : IBaseCommand;

}
