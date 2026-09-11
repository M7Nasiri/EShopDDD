using EShop.Query.MembershipAgg.DTOs;
using EShop.Shared.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.MembershipAgg.GetCustomerActiveMembership
{
    public record GetCustomerActiveMembershipQuery(Guid CustomerId) : IQuery<ActiveMembershipDto?>;

}
