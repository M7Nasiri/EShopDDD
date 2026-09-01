using _01.Domain.DoamainEvents.Payment;
using _01.Domain.DoamainEvents.Products;
using _01.Domain.Entities.Aggregates.OrderAgg.Repository;
using _01.Domain.Entities.Aggregates.ShipmentAgg;
using EShop.Shared.Application.Interfaces.Events;
using EShop.Shared.Application.Interfaces.Persistence;
using MassTransit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.PaymentAgg.EventHandlers
{
    public sealed class PaymentSucceededDomainEventHandler: INotificationHandler<PaymentSuccededDomainEvent>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublishEndpoint _publishEndpoint;
        public PaymentSucceededDomainEventHandler(IOrderRepository orderRepository,
            IUnitOfWork unitOfWork, IPublishEndpoint publishEndpoint)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Handle(PaymentSuccededDomainEvent notification, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetAsync(notification.OrderId, cancellationToken);
            if (order is null)
                return;

            order.MarkAsPaid();


            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _publishEndpoint.Publish(new PaymentSuccededIntegrationEvent(
                   PaymentId: notification.PaymentId,
                   OrderId: notification.OrderId,
                   Amount: notification.Amount.Amount,
                   OccuredOn:DateTime.UtcNow), cancellationToken);
        }

    }
}
