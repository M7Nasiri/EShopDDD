using _01.Domain.Entities.Aggregates.CartAgg;
using _01.Domain.Entities.Aggregates.CartAgg.Repository;
using _01.Domain.Entities.Aggregates.ProductAgg.Repository;
using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Application;
using EShop.Shared.Application.Interfaces.Authentication;
using EShop.Shared.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.CartAgg.Commands.AddItem
{
    public sealed class AddItemCommandHandler(
        ICartRepository cartRepository,
        IProductRepository productRepository,
        ICurrentUser currentUser,
        IUnitOfWork unitOfWork,
        IGuestSession guestSession)
        : IBaseCommandHandler<AddItemCommand>
    {
        public async Task<OperationResult> Handle(AddItemCommand request, CancellationToken cancellationToken)
        {

            var quantity = new Quantity(request.Quantity);

            var product = await productRepository.GetAsync(request.ProductId, cancellationToken);
            if (product is null)
                throw new EShopNullException("Product was not found.");

            if (product.Stock.Value < quantity.Value)
                throw new EShopDomainException("Requested quantity is not available in stock.");

            var cart = await GetOrCreateCartAsync(cancellationToken);

            cart.AddItem(request.ProductId, quantity, product.Stock);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return OperationResult.Success();
        }

        private async Task<Cart> GetOrCreateCartAsync(CancellationToken cancellationToken)
        {
            if (currentUser.IsValid() && currentUser.UserId.HasValue)
            {
                var customerId = currentUser.UserId.Value;
                var customerCart = await cartRepository.GetByCustomerIdAsync(customerId, cancellationToken);

                if (customerCart is null)
                {
                    customerCart = Cart.CreateForCustomer(customerId);
                    await cartRepository.AddAsync(customerCart, cancellationToken);
                }

                return customerCart;
            }

            var guestId = guestSession.GetOrCreateGuestId();
            var guestCart = await cartRepository.GetByGuestIdAsync(guestId, cancellationToken);

            if (guestCart is null)
            {
                guestCart = Cart.CreateForGuest(guestId);
                await cartRepository.AddAsync(guestCart, cancellationToken);
            }

            return guestCart;
        }
    }

}
