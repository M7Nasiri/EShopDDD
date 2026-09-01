using EShop.Shared.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.MembershipPlagAgg.Commands.ChangeMembershipPlanPrice
{
    public sealed record ChangeMembershipPlanPriceCommand(Guid PlanId,decimal NewPrice) : IBaseCommand;
}
