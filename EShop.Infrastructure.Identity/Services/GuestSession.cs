using EShop.Infrastructure.Identity.Constants;
using EShop.Shared.Application.Interfaces.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Infrastructure.Identity.Services
{
    public sealed class GuestSession : IGuestSession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GuestSession(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetOrCreateGuestId()
        {
            var httpContext = _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException(
                    "HttpContext is not available.");

            var existingGuestId = httpContext.Request.Cookies[
                Consts.GuestId];

            if (!string.IsNullOrWhiteSpace(existingGuestId))
                return existingGuestId.Trim();

            var guestId = Guid.NewGuid().ToString("N");

            httpContext.Response.Cookies.Append(
                Consts.GuestId,
                guestId,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false,
                    SameSite = SameSiteMode.Lax,
                    IsEssential = true,
                    Expires = DateTimeOffset.UtcNow.AddDays(30)
                });

            return guestId;
        }
        public string? GetGuestId()
        {
            return _httpContextAccessor.HttpContext?.Request.Cookies[Consts.GuestId];
        }

        public void Clear()
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete(Consts.GuestId);
        }
    }
}
