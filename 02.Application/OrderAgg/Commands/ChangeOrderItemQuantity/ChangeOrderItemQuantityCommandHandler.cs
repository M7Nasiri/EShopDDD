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

namespace _02.Application.OrderAgg.Commands.ChangeOrderItemQuantity
{
    public class ChangeOrderItemQuantityCommandHandler : IBaseCommandHandler<ChangeOrderItemQuantityCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;
        private readonly IProductRepository _productRepository;

        public ChangeOrderItemQuantityCommandHandler(
            IOrderRepository orderRepository,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser,
            IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _productRepository = productRepository;
        }
        public async Task<OperationResult> Handle(ChangeOrderItemQuantityCommand request, CancellationToken cancellationToken)
        {
            if (!_currentUser.IsValid())
            {
                throw new UnauthorizedAccessException(
                    "User must be authenticated.");
            }

            var customerId = _currentUser.UserId.Value;
            var productId = request.ProductId;

            var order = await _orderRepository.GetDraftOrderByCustomerIdAsync(customerId, cancellationToken);

            if (order is null)
                throw new EShopNullException("Order was not found"); 

            var product = await _productRepository.GetAsync(request.ProductId, cancellationToken);
            if (product == null)
                throw new EShopNullException("Product was not found.");

            if (product.Stock.Value < request.NewQuantity)
                throw new EShopDomainException("Quantity in stock is not enough.");


            order.ChangeItemQuantity(productId,new Quantity(request.NewQuantity));

            await _unitOfWork.SaveChangesAsync();
            return OperationResult.Success();

        }
    }
}
