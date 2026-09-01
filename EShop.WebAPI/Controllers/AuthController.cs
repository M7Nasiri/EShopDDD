using EShop.Shared.Application.Interfaces.Authentication;
using EShop.WebAPI.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EShop.WebAPI.Controllers;


[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private const string RefreshTokenCookieName = "eshop_refresh_token";

    private readonly IIdentityService _identityService;
    private readonly ITokenService _tokenService;

    public AuthController(
        IIdentityService identityService,
        ITokenService tokenService)
    {
        _identityService = identityService;
        _tokenService = tokenService;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest request)
    {
        var userId = await _identityService.RegisterCustomerAsync(
            request.UserName,
            request.Email,
            request.Password,
            request.FirstName,
            request.LastName);

        var tokens = await _tokenService.CreateTokensAsync(
            userId,
            GetIpAddress(),
            HttpContext.RequestAborted);

        SetRefreshTokenCookie(tokens.RefreshToken, tokens.RefreshTokenExpiresAtUtc);

        return Ok(new AuthResponse(tokens.AccessToken, tokens.AccessTokenExpiresAtUtc));
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        // خواندن توکن از کوکی مرورگر
        var refreshToken = Request.Cookies[RefreshTokenCookieName];

        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return Unauthorized(new { Message = "نشست کاری منقضی شده است. لطفا مجددا وارد شوید." });
        }

        var tokens = await _tokenService.RefreshTokensAsync(
            refreshToken,
            GetIpAddress(),
            HttpContext.RequestAborted);

        // ست کردن Refresh Token جدید (Token Rotation)
        SetRefreshTokenCookie(tokens.RefreshToken, tokens.RefreshTokenExpiresAtUtc);

        return Ok(new AuthResponse(tokens.AccessToken, tokens.AccessTokenExpiresAtUtc));
    }


    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request)
    {
        var userId = await _identityService.ValidateUserAsync(
            request.UserNameOrEmail,
            request.Password);

        if (userId is null)
        {
            return Unauthorized(new
            {
                Message = "Username/email or password is invalid."
            });
        }

        var tokens = await _tokenService.CreateTokensAsync(
            userId.Value,
            GetIpAddress(),
            HttpContext.RequestAborted);

        return Ok(tokens);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var refreshToken = Request.Cookies[RefreshTokenCookieName];

        if (!string.IsNullOrWhiteSpace(refreshToken))
        {
            await _tokenService.RevokeRefreshTokenAsync(
                refreshToken,
                GetIpAddress(),
                HttpContext.RequestAborted);
        }

        // پاک کردن کوکی از مرورگر
        DeleteRefreshTokenCookie();

        return NoContent();
    }

    #region Helper Methods

    // متد ست کردن کوکی
    private void SetRefreshTokenCookie(string refreshToken, DateTimeOffset expiresAtUtc)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true, // غیرقابل دسترسی با اسکریپت‌های کلاینت (جلوگیری از XSS)
            Secure = true,   // ارسال فقط روی بستر HTTPS (در Localhost هم کار می‌کند)
            SameSite = SameSiteMode.Lax, // جلوگیری از CSRF در ریکوئست‌های عادی
            Expires = expiresAtUtc,
            IsEssential = true, // نادیده گرفتن سیاست‌های عدم ردیابی GDPR
            Path = "/api/auth"  // اختیاری ولی بهینه: کوکی فقط به اندپوینت‌های Auth ارسال شود
        };

        Response.Cookies.Append(RefreshTokenCookieName, refreshToken, cookieOptions);
    }

    // متد حذف کوکی هنگام لاگ‌اوت
    private void DeleteRefreshTokenCookie()
    {
        Response.Cookies.Delete(RefreshTokenCookieName, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Path = "/api/auth"
        });
    }

    private string? GetIpAddress()
    {
        if (Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor))
        {
            return forwardedFor.FirstOrDefault()?.Split(',').FirstOrDefault()?.Trim();
        }

        return HttpContext.Connection.RemoteIpAddress?.ToString();
    }

    #endregion
}
