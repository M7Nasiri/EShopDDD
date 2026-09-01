using EShop.Shared.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.MembershipPlagAgg.Commands.ChangeMembershipPlanStatus
{
    public sealed record ChangeMembershipPlanStatusCommand(Guid PlanId, bool Enable) : IBaseCommand;
}
