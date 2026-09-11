using EShop.Infrastructure.Identity.Constants;
using EShop.Infrastructure.Identity.Entities;
using EShop.Shared.Application.Interfaces.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using EShop.Shared.Application.Exceptions;

namespace EShop.Infrastructure.Identity.Services
{
    public sealed class IdentityService : IIdentityService
    {
        private readonly UserManager<AppUser> _userManager;

        public IdentityService(
            UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Guid> RegisterCustomerAsync(
            string userName,
            string email,
            string phoneNumber,
            string password,
            string? firstName,
            string? lastName)
        {
            var user = new AppUser(
                userName,
                email,
                phoneNumber,
                firstName,
                lastName);

            var createResult = await _userManager.CreateAsync(
                user,
                password);

            if (!createResult.Succeeded)
            {
                var errors = string.Join(
                    " | ",
                    createResult.Errors.Select(x => x.Description));

                throw new InvalidOperationException(errors);
            }

            var roleResult = await _userManager.AddToRoleAsync(
                user,
                Roles.Customer);

            if (!roleResult.Succeeded)
            {
                var errors = string.Join(
                    " | ",
                    roleResult.Errors.Select(x => x.Description));

                throw new InvalidOperationException(errors);
            }
            return user.Id;
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                return;

            var user = await _userManager.FindByIdAsync(
                userId.ToString());

            // حذف جبرانی را Idempotent می‌کنیم:
            // اگر قبلاً حذف شده باشد، عملیات موفق تلقی می‌شود.
            if (user is null)
                return;

            var deleteResult = await _userManager.DeleteAsync(user);

            if (!deleteResult.Succeeded)
            {
                var errors = string.Join(
                    " | ",
                    deleteResult.Errors.Select(x => x.Description));
                throw new EShopIdentityException(errors);
            }
        }

        public async Task<Guid?> ValidateUserAsync(
            string userNameOrEmail,
            string password)
        {
            var normalizedValue = userNameOrEmail.Trim();

            var user = await _userManager.FindByNameAsync(
                normalizedValue);

            user ??= await _userManager.FindByEmailAsync(
                normalizedValue);

            if (user is null || !user.IsActive)
                return null;

            var passwordIsCorrect = await _userManager
                .CheckPasswordAsync(user, password);

            return passwordIsCorrect
                ? user.Id
                : null;
        }
    }
}
