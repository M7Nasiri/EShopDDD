using _01.Domain.Entities.Aggregates.CartAgg.Repository;
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

namespace _02.Application.CartAgg.Commands.RemoveItem
{
    public class RemoveItemCommandHandler : IBaseCommandHandler<RemoveItemCommand>
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;


        public RemoveItemCommandHandler(
           ICartRepository cartRepository,
           ICurrentUser currentUser,
           IProductRepository productRepository,
           IUnitOfWork unitOfWork)
        {
            _cartRepository = cartRepository;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
        }


        public async Task<OperationResult> Handle(RemoveItemCommand request, CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated ||
               _currentUser.UserId is null)
            {
                throw new UnauthorizedAccessException(
                    "User must be authenticated.");
            }

            var customerId = _currentUser.UserId.Value;
            var productId = request.ProductId;

            // 1) محصول را می‌خوانیم
            var product = await _productRepository.GetAsync(
                productId,
                cancellationToken);

            if (product is null)
                throw new EShopNullException("Product was not found.");

            var cart = await _cartRepository.GetByCustomerIdAsync(
                    customerId,
                    cancellationToken);

            if (cart is null)
                throw new EShopNullException("Cart does not exist");
            cart.RemoveItem(productId);

            // EF Core تغییرات Aggregate را Track کرده است
            await _unitOfWork.SaveChangesAsync(
                cancellationToken);
            return OperationResult.Success();
        }
    }
}
