using EShop.Shared.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.MembershipPlagAgg.Commands.ChangeMembershipPlanDiscount
{
    public sealed record ChangeMembershipPlanDiscountCommand(Guid PlanId,int NewDiscountPercent) : IBaseCommand;
}
