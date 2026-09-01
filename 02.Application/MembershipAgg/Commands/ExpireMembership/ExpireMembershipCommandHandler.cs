using _01.Domain.Entities.Aggregates.MembershipAgg.Repository;
using _01.Domain.ValueObjects;
using EShop.Shared.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.MembershipAgg.Commands.ExpireMembership
{
    public class ExpireMembershipCommandHandler : IBaseCommandHandler<ExpireMembershipCommand>
    {
        private readonly IMembershipRepository _membershipRepository;
        public ExpireMembershipCommandHandler(IMembershipRepository membershipRepository)
        {
            _membershipRepository = membershipRepository;
        }
        public async Task<OperationResult> Handle(ExpireMembershipCommand request, CancellationToken cancellationToken)
        {
            var now = DomainDate.Now;
            int totalExpiredCount = 0;

            while (!cancellationToken.IsCancellationRequested)
            {

                var batch = await _membershipRepository.GetExpiredActiveMembershipsBatchAsync(
                    now,
                    request.BatchSize,
                    cancellationToken);

                if (batch.Count == 0)
                    break;

                foreach (var membership in batch)
                {
                    membership.Expire();
                }

    
                await _membershipRepository.Save();

                totalExpiredCount += batch.Count;

                if (batch.Count < request.BatchSize)
                    break;
            }
            return OperationResult.Success();
        }
    }
}
