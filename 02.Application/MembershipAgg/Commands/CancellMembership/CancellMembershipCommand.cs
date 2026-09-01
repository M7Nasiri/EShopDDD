using EShop.Shared.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.MembershipAgg.Commands.CancellMembership
{
    public sealed record CancellMembershipCommand(Guid MembershipId) : IBaseCommand;
}
