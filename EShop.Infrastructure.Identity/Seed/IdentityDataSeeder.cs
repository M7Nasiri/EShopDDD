using EShop.Infrastructure.Identity.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Infrastructure.Identity.Seed
{
    public static class IdentityDataSeeder
    {
        public static async Task SeedRolesAsync(
       IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var roleManager = scope.ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            foreach (var roleName in Roles.All)
            {
                var exists = await roleManager.RoleExistsAsync(
                    roleName);

                if (exists)
                    continue;

                var createResult = await roleManager.CreateAsync(
                    new IdentityRole<Guid>(roleName));

                if (!createResult.Succeeded)
                {
                    var errors = string.Join(
                        " | ",
                        createResult.Errors.Select(
                            x => x.Description));

                    throw new InvalidOperationException(
                        $"Could not create role '{roleName}': {errors}");
                }
            }
        }
    }
}
