using System;
using System.Collections.Generic;
using System.Text;
using EShop.Query.MembershipAgg.DTOs;
using EShop.Shared.Query;

namespace EShop.Query.MembershipAgg.GetMembershipForAdmin
{
    public record GetMembershipsForAdminQuery(MembershipFilterParam FilterParams)
        : IQuery<MembershipFilterResult>;
}
