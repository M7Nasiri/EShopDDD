using _01.Domain.Entities.Aggregates.CartAgg.Repository;
using _01.Domain.Entities.Aggregates.CategoryAgg.Repository;
using _01.Domain.Entities.Aggregates.CommentAgg.Repository;
using _01.Domain.Entities.Aggregates.CouponAgg.Repository;
using _01.Domain.Entities.Aggregates.CustomerAgg.Repository;
using _01.Domain.Entities.Aggregates.MembershipAgg.Repository;
using _01.Domain.Entities.Aggregates.MembershipPlanAgg.Repository;
using _01.Domain.Entities.Aggregates.OrderAgg.Repository;
using _01.Domain.Entities.Aggregates.PaymentAgg.Repository;
using _01.Domain.Entities.Aggregates.ProductAgg.Repository;
using _01.Domain.Entities.Aggregates.ShipmentAgg.Repository;
using EShop.Infrastructure.Consumers;
using EShop.Infrastructure.ExternalServices.Payment;
using EShop.Infrastructure.PersistentEFCore;
using EShop.Infrastructure.PersistentEFCore.CartAgg;
using EShop.Infrastructure.PersistentEFCore.CategoryAgg;
using EShop.Infrastructure.PersistentEFCore.CommentAgg;
using EShop.Infrastructure.PersistentEFCore.CouponAgg;
using EShop.Infrastructure.PersistentEFCore.CustomerAgg;
using EShop.Infrastructure.PersistentEFCore.MembershipAgg;
using EShop.Infrastructure.PersistentEFCore.MembershipPlanAgg;
using EShop.Infrastructure.PersistentEFCore.OrderAgg;
using EShop.Infrastructure.PersistentEFCore.PaymentAgg;
using EShop.Infrastructure.PersistentEFCore.ProductAgg;
using EShop.Infrastructure.PersistentEFCore.ShipmentAgg;
using EShop.Shared.Application.Interfaces.ExternalServices;
using EShop.Shared.Application.Interfaces.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using EShop.Infrastructure.Persistent.Dapper;

namespace EShop.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services
            , IConfiguration configuration)
        {
            // ثبت کلاینت اختصاصی درگاه پرداخت
            services.AddHttpClient<IPaymentGatewayService, ZarinpalPaymentGatewayService>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            var connectionString = configuration.GetConnectionString(
                "ShopConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "ShopConnection was not configured.");
            }

            services.AddDbContext<ShopContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddTransient(_=>new DapperContext(connectionString));

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

            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ICouponRepository, CouponRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IMembershipRepository, MembershipRepository>();
            services.AddScoped<IMembershipPlanRepository, MembershipPlanRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IShipmentRepository, ShipmentRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
