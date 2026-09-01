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
    public sealed class AddItemCommandHandler
    : IBaseCommandHandler<AddItemCommand>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;

        public AddItemCommandHandler(
            ICartRepository cartRepository,
            IProductRepository productRepository,
            ICurrentUser currentUser,
            IUnitOfWork unitOfWork)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult> Handle(AddItemCommand request, CancellationToken cancellationToken)
        {
            if (!_currentUser.IsValid())
            {
                throw new UnauthorizedAccessException(
                    "User must be authenticated.");
            }

            var customerId = _currentUser.UserId.Value;



            var productId = request.ProductId;
            var quantity = new Quantity(request.Quantity);

            var product = await _productRepository.GetAsync(
                productId,
                cancellationToken);

            if (product is null)
                throw new EShopNullException("Product was not found.");


            if (product.Stock.Value < quantity.Value)
                throw new EShopDomainException(
                    "Requested quantity is not available in stock.");

            var cart =
                await _cartRepository.GetByCustomerIdAsync(
                    customerId,
                    cancellationToken);

            if (cart is null)
            {
                cart = new Cart(
                    customerId);

                await _cartRepository.AddAsync(
                    cart,
                    cancellationToken);
            }

            var existingItem = cart.Items.FirstOrDefault(
                x => x.ProductId == productId);

            var finalQuantity =
                (existingItem?.Quantity.Value ?? 0) +
                quantity.Value;

            if (product.Stock.Value < finalQuantity)
                throw new EShopDomainException(
                    "Total requested quantity is not available in stock.");

            var availableStock = product.Stock;
            cart.AddItem(productId, quantity, availableStock);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);
            return OperationResult.Success();
        }
    }

}
