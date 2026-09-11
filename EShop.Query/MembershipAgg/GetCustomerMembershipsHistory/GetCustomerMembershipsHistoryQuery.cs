using EShop.Query.MembershipAgg.DTOs;
using EShop.Shared.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.MembershipAgg.GetCustomerMembershipsHistory
{
    public record GetCustomerMembershipsHistoryQuery(Guid CustomerId) : IQuery<IReadOnlyList<CustomerMembershipHistoryDto>>;
}
