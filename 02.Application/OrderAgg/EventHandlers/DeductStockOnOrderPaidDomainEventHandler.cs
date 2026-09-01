using _01.Domain.DoamainEvents.Orders;
using _01.Domain.Entities.Aggregates.OrderAgg.Repository;
using _01.Domain.Entities.Aggregates.ProductAgg.Repository;
using _01.Domain.Exceptions;
using EShop.Shared.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.OrderAgg.EventHandlers
{
    public class DeductStockOnOrderPaidDomainEventHandler : INotificationHandler<OrderPaidDomainEvent>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeductStockOnOrderPaidDomainEventHandler(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(OrderPaidDomainEvent notification, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetWithItemsTrackingAsync(notification.OrderId, cancellationToken);
            if (order is null) return;

            // کسر موجودی تک‌تک محصولات خریداری شده در تراکنش دیتابیس
            foreach (var item in order.Items)
            {
                var product = await _productRepository.GetTracking(item.ProductId, cancellationToken);
                if (product is null)
                    throw new EShopDomainException($"محصول با شناسه {item.ProductId} یافت نشد.");

                // کاهش موجودی در دامین محصول
                product.DecreaseStock(item.Quantity);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);  
        }
    }
}
