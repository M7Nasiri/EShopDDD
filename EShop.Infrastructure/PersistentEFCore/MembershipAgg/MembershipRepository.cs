using _01.Domain.Consts;
using _01.Domain.Entities.Aggregates.MembershipAgg;
using _01.Domain.Entities.Aggregates.MembershipAgg.Repository;
using _01.Domain.Entities.Aggregates.MembershipPlanAgg;
using _01.Domain.ValueObjects;
using EShop.Infrastructure._Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Infrastructure.PersistentEFCore.MembershipAgg
{
    public class MembershipRepository : BaseRepository<Membership>, IMembershipRepository
    {
        private readonly ShopContext _dbContext;
        public MembershipRepository(ShopContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Membership?> GetActiveMembershipByCustomerIdAsync(_01.Domain.Guid customerId, CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            return await _dbContext.Set<Membership>().Where(ms => ms.CustomerId == customerId 
            && ms.Status == MembershipStatus.Active && ms.EndDate.Value > now)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Membership>> GetExpiredActiveMembershipsAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            return await _dbContext.Set<Membership>().Where(ms=>ms.Status == MembershipStatus.Active
            && ms.EndDate.Value <= now).ToListAsync(cancellationToken);
        }

        public async Task<List<Membership>> GetExpiredActiveMembershipsBatchAsync(DomainDate currentDate, int batchSize = 200, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<Membership>()
           .Where(m => m.Status == MembershipStatus.Active
                    && m.EndDate.Value <= currentDate.Value)
           .OrderBy(m => m.EndDate.Value) // اولویت با اشتراک‌هایی که زودتر منقضی شده‌اند
           .Take(batchSize)
           .ToListAsync(cancellationToken);
        }
    }
}
