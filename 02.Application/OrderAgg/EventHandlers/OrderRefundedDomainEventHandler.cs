using _01.Domain.DoamainEvents.Orders;
using _01.Domain.Entities.Aggregates.ProductAgg.Repository;
using _01.Domain.ValueObjects;
using EShop.Shared.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.OrderAgg.EventHandlers
{
    public class OrderRefundedDomainEventHandler : INotificationHandler<OrderCancelledDomainEvent>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderRefundedDomainEventHandler(
            IProductRepository productRepository,
            IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(
            OrderCancelledDomainEvent notification,
            CancellationToken cancellationToken)
        {
            if (notification.Items is null || !notification.Items.Any())
                return;

            foreach (var item in notification.Items)
            {
                var product = await _productRepository.GetAsync(item.ProductId.Value, cancellationToken);

                if (product is not null)
                {
                    product.IncreaseStock(new Quantity(item.Quantity));
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
