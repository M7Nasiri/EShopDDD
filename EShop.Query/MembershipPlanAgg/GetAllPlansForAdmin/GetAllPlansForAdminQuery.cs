using System;
using System.Collections.Generic;
using System.Text;
using EShop.Query.MembershipPlanAgg.DTOs.Admin;
using EShop.Shared.Query;

namespace EShop.Query.MembershipPlanAgg.GetAllPlansForAdmin
{
    public record GetAllPlansForAdminQuery(MembershipPlanFilterParam filterParams) 
    : IQuery<MembershipPlanFilterResult>
    {
    }
}
