using System;
using System.Collections.Generic;
using System.Text;
using EShop.Infrastructure.PersistentEFCore;
using EShop.Query.MembershipPlanAgg.DTOs.Admin;
using EShop.Shared.Query;
using Microsoft.EntityFrameworkCore;

namespace EShop.Query.MembershipPlanAgg.GetMembershipPlansLookup
{
    public class GetMembershipPlansLookupQueryHandler(ShopContext dbContext)
        : IQueryHandler<GetMembershipPlansLookupQuery, IReadOnlyList<MembershipPlanLookupDto>>
    {
        public async Task<IReadOnlyList<MembershipPlanLookupDto>> Handle(
            GetMembershipPlansLookupQuery request,
            CancellationToken cancellationToken)
        {
            return await dbContext.Plans
                .AsNoTracking()
                .Where(p => p.IsActive)
                .OrderBy(p => p.Price.Amount)
                .Select(p => new MembershipPlanLookupDto(
                    p.Id,
                    p.Name.Value,
                    p.Price.Amount
                ))
                .ToListAsync(cancellationToken);
        }
    }
}
