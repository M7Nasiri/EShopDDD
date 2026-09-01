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

namespace _02.Application.OrderAgg.Commands.RemoveMembershipDiscount
{
    internal class RemoveMembershipDiscountCommandHandler : IBaseCommandHandler<RemoveMembershipDiscountCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMembershipRepository _membershipRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveMembershipDiscountCommandHandler(
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
        public async Task<OperationResult> Handle(RemoveMembershipDiscountCommand request, CancellationToken cancellationToken)
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

            order.RemoveMembershipDiscount();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return OperationResult.Success();

        }
    }
}
