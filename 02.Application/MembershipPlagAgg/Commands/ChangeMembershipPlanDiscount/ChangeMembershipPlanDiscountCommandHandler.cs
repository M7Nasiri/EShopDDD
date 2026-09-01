using _01.Domain.Entities.Aggregates.MembershipPlanAgg.Repository;
using _01.Domain.Exceptions;
using EShop.Shared.Application;
using EShop.Shared.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.MembershipPlagAgg.Commands.ChangeMembershipPlanDiscount
{
    public class ChangeMembershipPlanPriceCommandHandler : IBaseCommandHandler<ChangeMembershipPlanPriceCommand>
    {
        private readonly IMembershipPlanRepository _membershipPlanRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ChangeMembershipPlanPriceCommandHandler(
             IMembershipPlanRepository membershipPlanRepository,
             IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _membershipPlanRepository = membershipPlanRepository;
        }

        public async Task<OperationResult> Handle(ChangeMembershipPlanPriceCommand request, CancellationToken cancellationToken)
        {
            var plan = await _membershipPlanRepository.GetTracking(request.PlanId, cancellationToken);
            if (plan == null)
                throw new EShopNullException("Plan was not found.");

            plan.ChangeDiscount(request.NewDiscountPercent);

            await _unitOfWork.SaveChangesAsync();
            return OperationResult.Success();
        }
    }
}
