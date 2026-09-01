using _01.Domain.Entities.Aggregates.CommentAgg.Repository;
using _01.Domain.Entities.Aggregates.MembershipAgg;
using _01.Domain.Entities.Aggregates.MembershipAgg.Repository;
using _01.Domain.Entities.Aggregates.MembershipPlanAgg.Repository;
using _01.Domain.Entities.Aggregates.ProductAgg.Repository;
using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Application;
using EShop.Shared.Application.Interfaces.Authentication;
using EShop.Shared.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.MembershipAgg.Commands.PurchaseMembership
{
    public class PurchaseMembershipCommandHandler : IBaseCommandHandler<PurchaseMembershipCommand>
    {
        private readonly IMembershipRepository _memberShipRepository;
        private readonly IMembershipPlanRepository _planRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;

        public PurchaseMembershipCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser,
            IMembershipRepository memberShipRepository,
            IMembershipPlanRepository planRepository)
        {
            
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _memberShipRepository = memberShipRepository;
            _planRepository = planRepository;
        }
        public async Task<OperationResult> Handle(PurchaseMembershipCommand request, CancellationToken cancellationToken)
        {
            if (!_currentUser.IsValid())
            {
                throw new UnauthorizedAccessException(
                    "User must be authenticated.");
            }

            var customerId = new Guid(_currentUser.UserId.Value);

            var activeMembership = _memberShipRepository.GetActiveMembershipByCustomerIdAsync(customerId, cancellationToken);
            if (activeMembership != null)
                throw new EShopDomainException("You have active plan.");

            var plan = await _planRepository.GetAsync(request.PlanId, cancellationToken);
            if (plan is null)
            {
                throw new EShopNullException("Plan was not found.");
            }

            var newMemberShip = new Membership(customerId, plan, DomainDate.Now);
            _memberShipRepository.Add(newMemberShip);
            
            await _unitOfWork.SaveChangesAsync();
            return OperationResult.Success();
        }
    }
}
