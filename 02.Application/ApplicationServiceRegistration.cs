using System;
using System.Collections.Generic;
using System.Text;
using _02.Application.CartAgg.Commands.AddItem;
using EShop.Shared.Application.FileUtil.Interfaces;
using EShop.Shared.Application.FileUtil.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace _02.Application
{
    public static class ApplicationServiceRegistration
    {
        public static void RegisterApplicationDependency(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(AddItemCommandValidator).Assembly);
            services.AddScoped<IFileService, FileService>();
            //services.AddMediatR(cfg =>
            //{
            //    cfg.RegisterServicesFromAssembly(typeof(AddItemCommand).Assembly);
            //});
        }
    }
}
