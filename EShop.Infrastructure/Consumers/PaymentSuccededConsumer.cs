using _01.Domain.Entities.Aggregates.OrderAgg;
using _01.Domain.Entities.Aggregates.OrderAgg.Repository;
using _01.Domain.Entities.Aggregates.ShipmentAgg;
using _01.Domain.Entities.Aggregates.ShipmentAgg.Repository;
using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Infrastructure.PersistentEFCore.OrderAgg;
using EShop.Infrastructure.PersistentEFCore.ShipmentAgg;
using EShop.Shared.Application.Interfaces.Events;
using EShop.Shared.Application.Interfaces.Persistence;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Infrastructure.Consumers
{
    public class PaymentSuccededConsumer : IConsumer<PaymentSuccededIntegrationEvent>
    {
        private readonly ILogger<PaymentSuccededConsumer> _logger;
        private readonly IShipmentRepository _shipmentRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentSuccededConsumer(
         ILogger<PaymentSuccededConsumer> logger,
         IShipmentRepository shipmentRepository,
         IOrderRepository orderRepository,
         IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _shipmentRepository = shipmentRepository;
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task Consume(ConsumeContext<PaymentSuccededIntegrationEvent> context)
        {
            var msg = context.Message;
            _logger.LogWarning($"سفارش با شماره {msg.OrderId} به مبلغ {msg.Amount} با کد پیگری {msg.PaymentId} با موفقیت ، پرداخت شد.");
            //Need ComeBack

            var existingShipment = await _shipmentRepository.GetByOrderIdAsync(new Guid(msg.OrderId), context.CancellationToken);
            if (existingShipment is not null)
            {
                _logger.LogWarning("مرسوله برای سفارش {OrderId} قبلاً ایجاد شده است.", msg.OrderId);
                return;
            }

            var order = await _orderRepository.GetAsync(msg.OrderId, context.CancellationToken);
            if (order is null)
            {
                _logger.LogError("سفارش با شناسه {OrderId} یافت نشد.", msg.OrderId);
                throw new EShopDomainException("سفارش با چنین شناسه‌ای پیدا نشد.");
            }

            var shipment = new Shipment(
                orderId: order.Id,
                address: order.ShippingAddress
            );

            _shipmentRepository.Add(shipment);
            await _unitOfWork.SaveChangesAsync(context.CancellationToken);

            _logger.LogInformation("مرسوله با شناسه {ShipmentId} برای سفارش {OrderId} با موفقیت ثبت گردید.", shipment.Id.Value, msg.OrderId);
        }


    }
}
