using _01.Domain.Consts;
using EShop.Query.MembershipAgg.DTOs;
using EShop.Shared.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using EShop.Infrastructure.PersistentEFCore;

namespace EShop.Query.MembershipAgg.GetCustomerActiveMembership
{
    internal class GetCustomerActiveMembershipQueryHandler(ShopContext dbContext)
        : IQueryHandler<GetCustomerActiveMembershipQuery, ActiveMembershipDto?>
    {
        public async Task<ActiveMembershipDto?> Handle(
            GetCustomerActiveMembershipQuery request,
            CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;

            var membership = await dbContext.Memberships
                .AsNoTracking()
                .Where(m => m.CustomerId == request.CustomerId &&
                            m.Status == MembershipStatus.Active &&
                            m.StartDate.Value <= now &&
                            m.EndDate.Value > now)
                .OrderByDescending(m => m.EndDate.Value)
                .Select(m => new
                {
                    m.Id,
                    m.PlanId,
                    PlanTitle = m.PlanTitle.Value,
                    m.DiscountPercent,
                    m.FreeShipping,
                    StartDate = m.StartDate.Value,
                    EndDate = m.EndDate.Value
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (membership is null)
                return null;

            var remainingDays = (membership.EndDate - now).Days;

            return new ActiveMembershipDto
            {
                MembershipId = membership.Id,
                PlanId = membership.PlanId,
                PlanTitle = membership.PlanTitle,
                DiscountPercent = membership.DiscountPercent,
                FreeShipping = membership.FreeShipping,
                StartDate = membership.StartDate,
                EndDate = membership.EndDate,
                RemainingDays = remainingDays < 0 ? 0 : remainingDays
            };
        }
    }
}
