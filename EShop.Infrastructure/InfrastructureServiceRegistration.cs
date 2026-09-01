using EShop.Infrastructure.Consumers;
using EShop.Infrastructure.ExternalServices.Payment;
using EShop.Shared.Application.Interfaces.ExternalServices;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // ثبت کلاینت اختصاصی درگاه پرداخت
            services.AddHttpClient<IPaymentGatewayService, ZarinpalPaymentGatewayService>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            });

          

            services.AddMassTransit(busConfigurator =>
            {
                // ۱. مشخص کردن فرمت نام‌گذاری اندپوینت‌ها به شکل کباب-کیس (مثلاً product-stock-low)
                busConfigurator.SetKebabCaseEndpointNameFormatter();

                // ۲. ثبت کانسومرهای پروژه به شکل خودکار یا دستی
                busConfigurator.AddConsumers(typeof(ProductStockLowConsumer).Assembly);

                // ۳. تنظیمات کانکشن RabbitMQ
                busConfigurator.UsingRabbitMq((context, cfg) =>
                {
                    var configuration = context.GetRequiredService<IConfiguration>();
                    var host = configuration["RabbitMq:Host"] ?? "localhost";
                    var user = configuration["RabbitMq:Username"] ?? "guest";
                    var pass = configuration["RabbitMq:Password"] ?? "guest";

                    cfg.Host(host, "/", h =>
                    {
                        h.Username(user);
                        h.Password(pass);
                    });

                    // ساخت صف‌ها و بایندینگ‌های خودکار
                    cfg.ConfigureEndpoints(context);
                });
            });



            return services;
        }
    }
}
