using _01.Domain.Entities.Aggregates.MembershipPlanAgg;
using _01.Domain.Entities.Aggregates.MembershipPlanAgg.Repository;
using _01.Domain.ValueObjects;
using EShop.Infrastructure._Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Infrastructure.PersistentEFCore.MembershipPlanAgg
{
    public class MembershipPlanRepository : BaseRepository<MembershipPlan>, IMembershipPlanRepository
    {
        private readonly ShopContext _dbContext;
        public MembershipPlanRepository(ShopContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<MembershipPlan>> GetActivePlansAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<MembershipPlan>().AsNoTracking().Where(p => p.IsActive).ToListAsync(cancellationToken);
        }
        public async Task<bool> IsNameUniqueAsync(string name, Guid? excludeId = null, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Set<MembershipPlan>().AsNoTracking().Where(mp => mp.Name.Value == name.Trim());
            if(excludeId != null)
            {
                query = query.Where(mp=>mp.Id != excludeId);
            }
            return !await query.AnyAsync(cancellationToken);
        }
    }
}
