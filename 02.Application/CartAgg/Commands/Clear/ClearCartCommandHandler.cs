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

namespace _02.Application.CartAgg.Commands.Clear
{
    public class ClearCartCommandHandler : IBaseCommandHandler<ClearCartCommand>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;

        public ClearCartCommandHandler(
            ICartRepository cartRepository,
            ICurrentUser currentUser,
            IUnitOfWork unitOfWork)
        {
            _cartRepository = cartRepository;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
        }
        public async Task<OperationResult> Handle(ClearCartCommand request, CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated ||
             _currentUser.UserId is null)
            {
                throw new UnauthorizedAccessException(
                    "User must be authenticated.");
            }

            var customerId = _currentUser.UserId.Value;
            var cart = await _cartRepository.GetByCustomerIdAsync(
                   customerId,
                   cancellationToken);

            if (cart is null)
                throw new EShopNullException("Cart does not exist");
            cart.Clear();

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);
            return OperationResult.Success();
        }
    }
}
