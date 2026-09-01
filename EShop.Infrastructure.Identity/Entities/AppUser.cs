using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Infrastructure.Identity.Entities
{
    public sealed class AppUser : IdentityUser<Guid>
    {
        public string? FirstName { get; private set; }

        public string? LastName { get; private set; }

        public bool IsActive { get; private set; } = true;

        // Navigation property برای Refresh Tokenها
        public ICollection<RefreshToken> RefreshTokens { get; private set; }
            = new List<RefreshToken>();

        private AppUser()
        {
            // For EF Core / ASP.NET Core Identity
        }

        public AppUser(
            string userName,
            string email,
            string phoneNumber,
            string? firstName = null,
            string? lastName = null)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException(
                    "User name is required.",
                    nameof(userName));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException(
                    "Email is required.",
                    nameof(email));

            Id = Guid.NewGuid();;

            UserName = userName.Trim();
            Email = email.Trim().ToLowerInvariant();
            PhoneNumber = phoneNumber;

            FirstName = firstName?.Trim();
            LastName = lastName?.Trim();

            IsActive = true;
        }

        public void ChangeProfile(
            string? firstName,
            string? lastName)
        {
            FirstName = firstName?.Trim();
            LastName = lastName?.Trim();
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void Activate()
        {
            IsActive = true;
        }
    }
}

