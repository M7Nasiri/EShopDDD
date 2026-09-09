using _01.Domain.Entities.Aggregates.CartAgg.Repository;
using _01.Domain.Entities.Aggregates.ProductAgg.Repository;
using EShop.Shared.Application;
using EShop.Shared.Application.Interfaces.Authentication;
using EShop.Shared.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.CartAgg.Commands.MergeGuestCart
{
    public class MergeGuestCartCommandHandler(
        ICartRepository cartRepository,
        IProductRepository productRepository,
        ICurrentUser currentUser,
        IGuestSession guestSession,
        IUnitOfWork unitOfWork)
        : IBaseCommandHandler<MergeGuestCartCommand>
    {
        
        public async Task<OperationResult> Handle(MergeGuestCartCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.IsValid() || !currentUser.UserId.HasValue)
                return OperationResult.Success(); 

            var guestId = guestSession.GetGuestId();
            if (string.IsNullOrWhiteSpace(guestId))
                return OperationResult.Success();

            var guestCart = await cartRepository.GetByGuestIdAsync(guestId, cancellationToken);
            if (guestCart is null || !guestCart.Items.Any())
            {
                guestSession.Clear();
                return OperationResult.Success();
            }

            var customerId = currentUser.UserId.Value;
            var customerCart = await cartRepository.GetByCustomerIdAsync(customerId, cancellationToken);

            if (customerCart is null)
            {
                guestCart.AssignToCustomer(customerId);
            }
            else
            {
                foreach (var guestItem in guestCart.Items)
                {
                    var product = await productRepository.GetAsync(guestItem.ProductId, cancellationToken);
                    if (product is null || product.Stock.Value <= 0)
                        continue;

                    customerCart.AddItem(guestItem.ProductId, guestItem.Quantity, product.Stock);
                }

                cartRepository.Remove(guestCart);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            guestSession.Clear(); 

            return OperationResult.Success();
        }
    }
}
