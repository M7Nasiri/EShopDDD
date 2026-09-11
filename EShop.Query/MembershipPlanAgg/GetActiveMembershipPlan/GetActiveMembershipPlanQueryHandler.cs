using System;
using System.Collections.Generic;
using System.Text;
using EShop.Infrastructure.PersistentEFCore;
using EShop.Query.MembershipPlanAgg.DTOs;
using EShop.Shared.Query;
using Microsoft.EntityFrameworkCore;

namespace EShop.Query.MembershipPlanAgg.GetActiveMembershipPlan
{
    public class GetActiveMembershipPlanQueryHandler(ShopContext dbContext) : IQueryHandler<GetActiveMembershipPlanQuery,IReadOnlyList<MembershipPlanDto>>
    {
        public async Task<IReadOnlyList<MembershipPlanDto>> Handle(GetActiveMembershipPlanQuery request, CancellationToken cancellationToken)
        {
            return await dbContext.Plans
                .AsNoTracking()
                .Where(p => p.IsActive)
                .OrderByDescending(p => p.CreationDate) 
                .Select(p => new MembershipPlanDto
                {
                    Id = p.Id,
                    Name = p.Name.Value,
                    Price = p.Price.Amount,
                    DiscountPercent = p.DiscountPercent,
                    FreeShipping = p.FreeShipping,
                    DurationInDays = p.DurationInDays.Days
                })
                .ToListAsync(cancellationToken);
        }
    }
}
