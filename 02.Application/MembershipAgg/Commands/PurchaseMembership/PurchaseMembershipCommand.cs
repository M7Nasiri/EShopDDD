using EShop.Shared.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.MembershipAgg.Commands.PurchaseMembership
{
    public sealed record PurchaseMembershipCommand(Guid PlanId) : IBaseCommand;
}
