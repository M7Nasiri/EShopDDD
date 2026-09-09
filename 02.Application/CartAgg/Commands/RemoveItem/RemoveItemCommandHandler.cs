using _01.Domain.Entities.Aggregates.CartAgg;
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
    public class RemoveItemCommandHandler(
        ICartRepository cartRepository,
        ICurrentUser currentUser,
        IProductRepository productRepository,
        IGuestSession guestSession,
        IUnitOfWork unitOfWork)
        : IBaseCommandHandler<RemoveItemCommand>
    {
        private readonly ICartRepository _cartRepository = cartRepository;
        private readonly ICurrentUser _currentUser = currentUser;


        public async Task<OperationResult> Handle(RemoveItemCommand request, CancellationToken cancellationToken)
        {
            
            var productId = request.ProductId;

            // 1) محصول را می‌خوانیم
            var product = await productRepository.GetAsync(
                productId,
                cancellationToken);

            if (product is null)
                throw new EShopNullException("Product was not found.");

            var cart = await GetOrCreateCartAsync(
                    cancellationToken);

            if (cart is null)
                throw new EShopNullException("Cart does not exist");
            cart?.RemoveItem(productId);

            // EF Core تغییرات Aggregate را Track کرده است
            await unitOfWork.SaveChangesAsync(
                cancellationToken);
            return OperationResult.Success();
        }

        private async Task<Cart?> GetOrCreateCartAsync(CancellationToken cancellationToken)
        {
            if (currentUser.IsValid() && currentUser.UserId.HasValue)
            {
                var customerId = currentUser.UserId.Value;
                var customerCart = await cartRepository.GetByCustomerIdAsync(customerId, cancellationToken);

                return customerCart;
            }

            var guestId = guestSession.GetOrCreateGuestId();
            var guestCart = await cartRepository.GetByGuestIdAsync(guestId, cancellationToken);

            return guestCart;
        }
    }
}
