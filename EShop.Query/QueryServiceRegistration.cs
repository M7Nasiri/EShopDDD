using System;
using System.Collections.Generic;
using System.Text;
using EShop.Query.CartAgg.GetCartByCustomerId;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace EShop.Query
{
    public static class QueryServiceRegistration 
    {
        public static void RegistrationQueryServices(this IServiceCollection services)
        {
            //services.AddMediatR(cfg =>
            //{
            //    cfg.RegisterServicesFromAssembly(typeof(GetCartByCustomerIdQuery).Assembly);
            //});
        }
    }
}
