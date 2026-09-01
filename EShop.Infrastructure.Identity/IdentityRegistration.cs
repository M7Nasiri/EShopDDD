using EShop.Infrastructure.Identity.Entities;
using EShop.Infrastructure.Identity.Options;
using EShop.Infrastructure.Identity.Persistence;
using EShop.Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using EShop.Shared.Application.Interfaces.Authentication;

namespace EShop.Infrastructure.Identity
{
    public static class IdentityRegistration
    {
        public static IServiceCollection AddIdentityInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(
                "IdentityConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "IdentityConnection was not configured.");
            }

            services.AddDbContext<EShopIdentityDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.Configure<JwtOptions>(
                configuration.GetSection(
                    JwtOptions.SectionName));

            services
                .AddIdentityCore<AppUser>(options =>
                {
                    options.User.RequireUniqueEmail = true;

                    options.Password.RequiredLength = 4;
                    //options.Password.RequireDigit = true;
                    //options.Password.RequireUppercase = true;
                    //options.Password.RequireLowercase = true;
                    //options.Password.RequireNonAlphanumeric = false;

                    //options.Lockout.AllowedForNewUsers = true;
                    //options.Lockout.MaxFailedAccessAttempts = 5;
                    //options.Lockout.DefaultLockoutTimeSpan =
                    //    TimeSpan.FromMinutes(15);
                })
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<EShopIdentityDbContext>()
                .AddSignInManager<SignInManager<AppUser>>()
                .AddDefaultTokenProviders();






            services.AddHttpContextAccessor();

            services.AddScoped<ICurrentUser, CurrentUser>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<ITokenService, JwtTokenService>();

            return services;
        }
    }
}
