using _01.Domain.Entities.Aggregates.MembershipAgg.Repository;
using _01.Domain.Entities.Aggregates.OrderAgg.Repository;
using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Application;
using EShop.Shared.Application.Interfaces.Authentication;
using EShop.Shared.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.OrderAgg.Commands.ApplyMembershipDiscount
{
    public class ApplyMembershipDiscountCommandHandler : IBaseCommandHandler<ApplyMembershipDiscountCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMembershipRepository _membershipRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;

        public ApplyMembershipDiscountCommandHandler(
             IUnitOfWork unitOfWork,
            IOrderRepository orderRepository,
            IMembershipRepository membershipRepository,
            ICurrentUser currentUser)
        {
            _orderRepository = orderRepository;
            _membershipRepository = membershipRepository;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
        }
        public async Task<OperationResult> Handle(ApplyMembershipDiscountCommand request, CancellationToken cancellationToken)
        {
            if (!_currentUser.IsValid())
            {
                throw new UnauthorizedAccessException(
                    "User must be authenticated.");
            }

            var customerId = new Guid(_currentUser.UserId.Value);

            var order = await _orderRepository.GetDraftOrderByCustomerIdAsync(customerId, cancellationToken);

            if (order is null)
                throw new EShopNullException("Order was not found");

            var activeMembership =await _membershipRepository.GetActiveMembershipByCustomerIdAsync(customerId,cancellationToken);
            if (activeMembership is null)
                throw new EShopDomainException("You have not active membership");

            var discountSnapShot = new DiscountSnapshot(activeMembership.PlanTitle.Value, activeMembership.DiscountPercent,activeMembership.FreeShipping);
            
            order.ApplyMembershipDiscount(discountSnapShot);
            await _unitOfWork.SaveChangesAsync();
            return OperationResult.Success();
        }
    }
}
