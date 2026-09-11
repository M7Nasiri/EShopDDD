using System;
using System.Collections.Generic;
using System.Text;
using _01.Domain.Consts;
using EShop.Infrastructure.PersistentEFCore;
using EShop.Query.MembershipAgg.DTOs;
using EShop.Shared.Query;
using Microsoft.EntityFrameworkCore;

namespace EShop.Query.MembershipAgg.GetCustomerMembershipsHistory
{
    public class GetCustomerMembershipsHistoryQueryHandler(ShopContext dbContext)
        : IQueryHandler<GetCustomerMembershipsHistoryQuery, IReadOnlyList<CustomerMembershipHistoryDto>>
    {
        public async Task<IReadOnlyList<CustomerMembershipHistoryDto>> Handle(
            GetCustomerMembershipsHistoryQuery request,
            CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;

            var list = await dbContext.Memberships
                .AsNoTracking()
                .Where(m => m.CustomerId == request.CustomerId)
                .OrderByDescending(m => m.StartDate.Value)
                .Select(m => new CustomerMembershipHistoryDto
                {
                    Id = m.Id,
                    PlanTitle = m.PlanTitle.Value,
                    DiscountPercent = m.DiscountPercent,
                    FreeShipping = m.FreeShipping,
                    StartDate = m.StartDate.Value,
                    EndDate = m.EndDate.Value,
                    Status = m.Status.ToString(),
                    IsCurrentlyActive = m.Status == MembershipStatus.Active && m.StartDate.Value <= now &&
                                        m.EndDate.Value > now
                })
                .ToListAsync(cancellationToken);
            return list;
        }
    }
}
