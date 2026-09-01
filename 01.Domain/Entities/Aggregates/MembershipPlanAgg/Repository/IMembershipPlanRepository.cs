using _01.Domain.ValueObjects;
using EShop.Shared.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Entities.Aggregates.MembershipPlanAgg.Repository
{
    public interface IMembershipPlanRepository : IBaseRepository<MembershipPlan>
    {
        Task<bool> IsNameUniqueAsync(string name, Guid? excludeId = null, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<MembershipPlan>> GetActivePlansAsync(CancellationToken cancellationToken = default);
    }
}
