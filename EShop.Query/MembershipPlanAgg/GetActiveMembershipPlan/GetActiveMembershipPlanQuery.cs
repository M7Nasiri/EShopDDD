using EShop.Query.MembershipPlanAgg.DTOs;
using EShop.Shared.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.MembershipPlanAgg.GetActiveMembershipPlan
{
    public record GetActiveMembershipPlanQuery : IQuery<IReadOnlyList<MembershipPlanDto>>;

}
