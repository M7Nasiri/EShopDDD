using _01.Domain.Entities.Aggregates.MembershipPlanAgg.Repository;
using _01.Domain.Exceptions;
using EShop.Shared.Application;
using EShop.Shared.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.MembershipPlagAgg.Commands.ChangeMembershipPlanStatus
{
    public class ChangeMembershipPlanStatusCommandHandler : IBaseCommandHandler<ChangeMembershipPlanStatusCommand>
    {
        private readonly IMembershipPlanRepository _membershipPlanRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ChangeMembershipPlanStatusCommandHandler(
             IMembershipPlanRepository membershipPlanRepository,
             IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _membershipPlanRepository = membershipPlanRepository;
        }
        public async Task<OperationResult> Handle(ChangeMembershipPlanStatusCommand request, CancellationToken cancellationToken)
        {
            var plan = await _membershipPlanRepository.GetTracking(request.PlanId, cancellationToken);
            if (plan == null)
                throw new EShopNullException("Plan was not found.");
            if (request.Enable)
                plan.Enable();
            else
                plan.Disable();

            await _unitOfWork.SaveChangesAsync();
            return OperationResult.Success();
        }
    }
}
