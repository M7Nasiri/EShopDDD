using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using EShop.Facade.Cart;
using EShop.Facade.Category;
using EShop.Facade.Comment;
using EShop.Facade.Coupon;
using EShop.Facade.Customer;
using EShop.Facade.Membership;
using EShop.Facade.MembershipPlan;
using EShop.Facade.Order;
using EShop.Facade.Payment;
using EShop.Facade.Product;
using EShop.Facade.Shipment;
using MassTransit.MessageData;
using Microsoft.Extensions.DependencyInjection;

namespace EShop.Facade
{
    public static class RegisterFacade
    {
        public static void InitFacade(this IServiceCollection services)
        {
            services.AddScoped<ICartFacade, CartFacade>();
            services.AddScoped<ICategoryFacade, CategoryFacade>();
            services.AddScoped<ICommentFacade, CommentFacade>();
            services.AddScoped<ICouponFacade, CouponFacade>();
            services.AddScoped<ICustomerFacade, CustomerFacade>();
            services.AddScoped<IMembershipFacade, MembershipFacade>();
            services.AddScoped<IMembershipPlanFacade, MembershipPlanFacade>();
            services.AddScoped<IOrderFacade, OrderFacade>();
            services.AddScoped<IPaymentFacade, PaymentFacade>();
            services.AddScoped<IProductFacade, ProductFacade>();
            services.AddScoped<IShipmentFacade, ShipmentFacade>();

        }
    }
}
