using _01.Domain.Entities.Aggregates.MembershipPlanAgg;
using _01.Domain.ValueObjects;
using EShop.Shared.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Entities.Aggregates.MembershipAgg.Repository
{
    public interface IMembershipRepository : IBaseRepository<Membership>
    {
        Task<Membership?> GetActiveMembershipByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Membership>> GetExpiredActiveMembershipsAsync(CancellationToken cancellationToken = default);
        Task<List<Membership>> GetExpiredActiveMembershipsBatchAsync(
            DomainDate currentDate,
            int batchSize = 200,
            CancellationToken cancellationToken = default);
    }
}
