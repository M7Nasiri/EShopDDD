using _01.Domain.Entities.Aggregates.MembershipAgg.Repository;
using _01.Domain.Entities.Aggregates.MembershipPlanAgg.Repository;
using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Application;
using EShop.Shared.Application.Interfaces.Authentication;
using EShop.Shared.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.MembershipAgg.Commands.CancellMembership
{

    public class CancellMembershipCommandHandler : IBaseCommandHandler<CancellMembershipCommand>
    {
        private readonly IMembershipRepository _memberShipRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;

        public CancellMembershipCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser,
            IMembershipRepository memberShipRepository)
        {

            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _memberShipRepository = memberShipRepository;
        }
        public async Task<OperationResult> Handle(CancellMembershipCommand request, CancellationToken cancellationToken)
        {
            if (!_currentUser.IsValid())
            {
                throw new UnauthorizedAccessException(
                    "User must be authenticated.");
            }

            var customerId = _currentUser.UserId.Value;
            var membership = await _memberShipRepository.GetTracking(request.MembershipId, cancellationToken);

            if (membership == null)
                throw new EShopNullException("Membership was not found.");

            if (membership.CustomerId != customerId || !_currentUser.IsAdmin)
                throw new EShopDomainException("You can not cancell this membership");

            membership.Cancel();

            await _unitOfWork.SaveChangesAsync();
            return OperationResult.Success();

        }
    }
}
