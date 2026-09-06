using _01.Domain.DomainEvents.Payment;
using _01.Domain.Entities.Aggregates.OrderAgg.Repository;
using _01.Domain.Entities.Aggregates.PaymentAgg.Repository;
using EShop.Shared.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.PaymentAgg.EventHandlers
{
    public class PaymentRefundedDomainEventHandler : INotificationHandler<PaymentRefundedDomainEvent>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUnitOfWork _unitOfWork;
        public PaymentRefundedDomainEventHandler(IOrderRepository orderRepository,
            IPaymentRepository paymentRepository,
            IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _paymentRepository = paymentRepository;
        }

        public async Task Handle(PaymentRefundedDomainEvent notification, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetAsync(notification.OrderId, cancellationToken);
            if (order is null)
                return;

            order.MarkAsRefunded();

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
