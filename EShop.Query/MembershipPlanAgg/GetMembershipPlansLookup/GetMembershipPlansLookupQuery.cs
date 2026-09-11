using EShop.Query.MembershipPlanAgg.DTOs.Admin;
using EShop.Shared.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.MembershipPlanAgg.GetMembershipPlansLookup
{
    public record GetMembershipPlansLookupQuery : IQuery<IReadOnlyList<MembershipPlanLookupDto>>;
}
