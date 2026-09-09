using System;
using System.Collections.Generic;
using System.Text;
using _02.Application.CartAgg.Commands.AddItem;
using EShop.Infrastructure.Identity.Services;
using EShop.Query.CartAgg.GetCartByCustomerId;
using EShop.Shared.Application.Interfaces.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Shop.Config
{
    public static class ServiceRegistration
    {
        public static void InitConfig(this IServiceCollection services)
        {
            services.AddScoped<ICurrentUser, CurrentUser>();
            services.AddScoped<IGuestSession, GuestSession>();
            services.AddMediatR(cfg =>
            {
                // اسکن تمام هندلرهای Command
                cfg.RegisterServicesFromAssembly(typeof(AddItemCommand).Assembly);

                // اسکن تمام هندلرهای Query
                cfg.RegisterServicesFromAssembly(typeof(GetCartByCustomerIdQuery).Assembly);
            });
        }
    }
}
