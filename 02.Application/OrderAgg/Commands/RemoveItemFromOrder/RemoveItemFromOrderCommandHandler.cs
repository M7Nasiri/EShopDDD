using _01.Domain.Entities.Aggregates.CommentAgg.Repository;
using _01.Domain.Entities.Aggregates.OrderAgg.Repository;
using _01.Domain.Entities.Aggregates.ProductAgg.Repository;
using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Application;
using EShop.Shared.Application.Interfaces.Authentication;
using EShop.Shared.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.OrderAgg.Commands.RemoveItemFromOrder
{
    public class RemoveItemFromOrderCommandHandler : IBaseCommandHandler<RemoveItemFromOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;


        public RemoveItemFromOrderCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser,
            IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }
        public async Task<OperationResult> Handle(RemoveItemFromOrderCommand request, CancellationToken cancellationToken)
        {
            if (!_currentUser.IsValid())
            {
                throw new UnauthorizedAccessException(
                    "User must be authenticated.");
            }

            var customerId = new Guid(_currentUser.UserId.Value);
            var productId = new Guid(request.ProductId);

            var order = await _orderRepository.GetDraftOrderByCustomerIdAsync(customerId, cancellationToken);

            if (order is null)
                throw new EShopNullException("Order was not found");

            order.RemoveItem(productId);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return OperationResult.Success();

        }
    }
}
