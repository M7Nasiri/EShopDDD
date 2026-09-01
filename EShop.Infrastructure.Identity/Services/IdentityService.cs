using EShop.Infrastructure.Identity.Constants;
using EShop.Infrastructure.Identity.Entities;
using EShop.Shared.Application.Interfaces.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

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
            string password,
            string? firstName,
            string? lastName)
        {
            var user = new AppUser(
                userName,
                email,
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
