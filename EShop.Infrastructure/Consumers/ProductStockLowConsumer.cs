using EShop.Shared.Application.Interfaces.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Infrastructure.Consumers
{
    public sealed class ProductStockLowConsumer : IConsumer<ProductStockLowIntegrationEvent>
    {
        private readonly ILogger<ProductStockLowConsumer> _logger;

        public ProductStockLowConsumer(ILogger<ProductStockLowConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ProductStockLowIntegrationEvent> context)
        {
            var msg = context.Message;
            _logger.LogWarning("هشدار کسری موجودی کالا: شناسه {ProductId} - موجودی فعلی: {Stock}",
                msg.ProductId, msg.CurrentStock);
            //Need ComeBack
            // در اینجا کد ارسال ایمیل، پیامک به انباردار یا اطلاع‌رسانی قرار می‌گیرد
            await Task.CompletedTask;
        }
    }
}
