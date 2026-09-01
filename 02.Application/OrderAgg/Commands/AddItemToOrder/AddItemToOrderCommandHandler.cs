using _01.Domain.Entities.Aggregates.OrderAgg;
using _01.Domain.Entities.Aggregates.OrderAgg.Repository;
using _01.Domain.Entities.Aggregates.ProductAgg;
using _01.Domain.Entities.Aggregates.ProductAgg.Repository;
using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Application;
using EShop.Shared.Application.Interfaces.Authentication;
using EShop.Shared.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.OrderAgg.Commands.AddItemToOrder
{
    public class AddItemToOrderCommandHandler : IBaseCommandHandler<AddItemToOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;

        public AddItemToOrderCommandHandler(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
        }
        public async Task<OperationResult> Handle(AddItemToOrderCommand request, CancellationToken cancellationToken)
        {
            if (!_currentUser.IsValid())
            {
                throw new UnauthorizedAccessException(
                    "User must be authenticated.");
            }

            var customerId = new Guid(_currentUser.UserId.Value);
            var productId = new Guid(request.ProductId);

            var product =await _productRepository.GetTracking(request.ProductId, cancellationToken);

            if(product is null)
            {
                throw new EShopNullException("Product does not exist");
            }

            if (product.Stock.Value < request.Quantity)
                throw new EShopDomainException("Product has not enough quantity.");

            var order =await _orderRepository.GetDraftOrderByCustomerIdAsync(customerId,cancellationToken);
            if (order is null)
            {
                order = new Order(customerId);
                await _orderRepository.AddAsync(order, cancellationToken);
            }

            var existingItem = order.Items.FirstOrDefault(x => x.ProductId == productId);
            var currentQuantityInCart = existingItem?.Quantity.Value ?? 0;
            var totalRequestedQuantity = currentQuantityInCart + request.Quantity;

            if (product.Stock.Value < totalRequestedQuantity)
                throw new EShopDomainException("Quantity in stock has not been enough.");

            order.AddItem(productId, new Quantity(request.Quantity),product.UnitPrice);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return OperationResult.Success();
        }
    }
}
