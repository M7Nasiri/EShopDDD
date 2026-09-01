using _01.Domain.Entities.Aggregates.OrderAgg.Repository;
using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Application;
using EShop.Shared.Application.Interfaces.Authentication;
using EShop.Shared.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.OrderAgg.Commands.RemoveCouponFromOrder
{
    public class RemoveCouponFromOrderCommandHandler : IBaseCommandHandler<RemoveCouponFromOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveCouponFromOrderCommandHandler(IOrderRepository orderRepository,
             IUnitOfWork unitOfWork,
            ICurrentUser currentUser)
        {
            _orderRepository = orderRepository;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult> Handle(RemoveCouponFromOrderCommand request, CancellationToken cancellationToken)
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

            order.RemoveCoupon();

            await _unitOfWork.SaveChangesAsync();

            return OperationResult.Success();

        }
    }
}
