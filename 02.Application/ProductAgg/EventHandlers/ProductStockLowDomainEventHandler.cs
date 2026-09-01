using _01.Domain.DoamainEvents.Products;
using EShop.Shared.Application.Interfaces.Events;
using MassTransit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.ProductAgg.EventHandlers
{
    public class ProductStockLowDomainEventHandler : INotificationHandler<ProductStockLowDomainEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public ProductStockLowDomainEventHandler(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Handle(ProductStockLowDomainEvent notification, CancellationToken cancellationToken)
        {
            // انتشار در RabbitMQ
            await _publishEndpoint.Publish(new ProductStockLowIntegrationEvent(
                ProductId: notification.ProductId.Value,
                CurrentStock: notification.CurrentStock,
                OccurredOn: DateTime.UtcNow), cancellationToken);
        }
    }
}
