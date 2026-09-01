using EShop.Infrastructure.Identity.Constants;
using EShop.Shared.Application.Interfaces.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using System.Text;

namespace EShop.Infrastructure.Identity.Services
{
    public sealed class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private HttpContext? HttpContext =>
            _httpContextAccessor.HttpContext;

        public bool IsAuthenticated =>
            HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public Guid? UserId
        {
            get
            {
                if (!IsAuthenticated)
                    return null;

                var userIdValue =
                    HttpContext?.User.FindFirstValue(
                        ClaimTypes.NameIdentifier);

                return Guid.TryParse(
                    userIdValue,
                    out var userId)
                    ? userId
                    : null;
            }
        }

        public string? GuestId
        {
            get
            {
                if (IsAuthenticated)
                    return null;

                var guestId = HttpContext?.Request.Cookies[
                    Consts.GuestId];

                return string.IsNullOrWhiteSpace(guestId)
                    ? null
                    : guestId.Trim();
            }
        }

        public bool IsAdmin =>
         IsAuthenticated &&
         (HttpContext?.User.IsInRole(Roles.Admin) ?? false);

        public bool CanModerateComments =>
            IsAuthenticated &&
            (
                HttpContext?.User.IsInRole(Roles.Admin) == true ||
                HttpContext?.User.IsInRole(Roles.Moderator) == true
            );

        public bool IsValid()
        {
            if (!IsAuthenticated ||
             UserId is null)
            {
                return false;
            }
            return true;
        }

        public bool IsInRole(string role)
        {
            return _httpContextAccessor.HttpContext?.User
                .IsInRole(role) ?? false;
        }
    }
}
