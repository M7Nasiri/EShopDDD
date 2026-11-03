using EShop.Infrastructure.Identity.Constants;
using EShop.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
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
        public static async Task SeedAdminAsync(
            IServiceProvider serviceProvider,
            IConfiguration configuration)
        {
            if (!configuration.GetValue<bool>("SeedAdmin:Enabled"))
                return;

            var userName = configuration["SeedAdmin:UserName"];
            var email = configuration["SeedAdmin:Email"];
            var phone = configuration["SeedAdmin:Phone"];
            var password = configuration["SeedAdmin:Password"];

            if (string.IsNullOrWhiteSpace(userName) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(phone) ||
                string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidOperationException(
                    "SeedAdmin configuration is incomplete.");
            }

            using var scope = serviceProvider.CreateScope();

            var userManager = scope.ServiceProvider
                .GetRequiredService<UserManager<AppUser>>();

            // اگر نام کاربری موجود باشد، هرگز بی‌سروصدا آن را Admin نکن.
            var existing = await userManager.FindByNameAsync(userName);

            if (existing is not null)
            {
                if (!await userManager.IsInRoleAsync(
                        existing, Roles.Admin))
                {
                    throw new InvalidOperationException(
                        "SeedAdmin username already exists without Admin role.");
                }

                return; // اجرای دوبارهٔ seed مشکلی ایجاد نمی‌کند.
            }

            if (await userManager.FindByEmailAsync(email) is not null)
            {
                throw new InvalidOperationException(
                    "SeedAdmin email already belongs to another user.");
            }

            // ترتیب آرگومان‌ها را با سازندهٔ AppUser خودت تطبیق بده.
            var admin = new AppUser(
                userName,
                email,
                phone,
                "System",
                "Administrator");

            var create = await userManager.CreateAsync(
                admin, password);

            if (!create.Succeeded)
            {
                throw new InvalidOperationException(
                    "Admin creation failed: " +
                    string.Join(" | ",
                        create.Errors.Select(e => e.Description)));
            }

            var addRole = await userManager.AddToRoleAsync(
                admin, Roles.Admin);

            if (!addRole.Succeeded)
            {
                // برای seed اولیه؛ در سناریوی حرفه‌ای‌تر تراکنش و
                // رسیدگی به خطای rollback هم لازم است.
                await userManager.DeleteAsync(admin);

                throw new InvalidOperationException(
                    "Assigning Admin role failed: " +
                    string.Join(" | ",
                        addRole.Errors.Select(e => e.Description)));
            }
        }

    }
}
