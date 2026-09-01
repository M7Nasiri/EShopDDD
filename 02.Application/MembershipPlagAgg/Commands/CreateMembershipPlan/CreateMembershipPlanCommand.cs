using EShop.Shared.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.MembershipPlagAgg.Commands.CreateMembershipPlan
{
    public sealed record CreateMembershipPlanCommand(string Name, decimal Price, int Percent, bool FreeShipping
        , int DurationInDays) : IBaseCommand;
}
