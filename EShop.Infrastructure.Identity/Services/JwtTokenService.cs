using EShop.Infrastructure.Identity.Entities;
using EShop.Infrastructure.Identity.Options;
using EShop.Infrastructure.Identity.Persistence;
using EShop.Shared.Application.Interfaces.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EShop.Infrastructure.Identity.Services
{
    public sealed class JwtTokenService : ITokenService
    {
        private readonly EShopIdentityDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly JwtOptions _jwtOptions;

        public JwtTokenService(
            EShopIdentityDbContext context,
            UserManager<AppUser> userManager,
            IOptions<JwtOptions> jwtOptions)
        {
            _context = context;
            _userManager = userManager;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<TokenResult> CreateTokensAsync(
            Guid userId,
            string? ipAddress,
            CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(
                userId.ToString());

            if (user is null || !user.IsActive)
            {
                throw new UnauthorizedAccessException(
                    "User is not available.");
            }

            return await CreateTokensForUserAsync(
                user,
                ipAddress,
                cancellationToken);
        }

        public async Task<TokenResult> RefreshTokensAsync(
            string refreshToken,
            string? ipAddress,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                throw new UnauthorizedAccessException(
                    "Refresh token is required.");
            }

            var refreshTokenHash = ComputeSha256(refreshToken);

            var storedRefreshToken = await _context.RefreshTokens
                .Include(x => x.User)
                .SingleOrDefaultAsync(
                    x => x.TokenHash == refreshTokenHash,
                    cancellationToken);

            if (storedRefreshToken is null || !storedRefreshToken.IsActive)
            {
                throw new UnauthorizedAccessException(
                    "Refresh token is invalid, expired, or revoked.");
            }

            if (!storedRefreshToken.User.IsActive)
            {
                throw new UnauthorizedAccessException(
                    "User is not active.");
            }

            /*
             Token Rotation:
             1. Refresh Token قدیمی revoke می‌شود.
             2. Access Token و Refresh Token جدید ساخته می‌شود.
             */
            var newRawRefreshToken = GenerateSecureToken();
            var newRefreshTokenHash = ComputeSha256(
                newRawRefreshToken);

            storedRefreshToken.Revoke(
                ipAddress,
                newRefreshTokenHash);

            var accessTokenExpiresAtUtc =
                DateTimeOffset.UtcNow.AddMinutes(
                    _jwtOptions.AccessTokenMinutes);

            var refreshTokenExpiresAtUtc =
                DateTimeOffset.UtcNow.AddDays(
                    _jwtOptions.RefreshTokenDays);

            var newRefreshToken = new RefreshToken(
                newRefreshTokenHash,
                storedRefreshToken.UserId,
                refreshTokenExpiresAtUtc,
                ipAddress);

            _context.RefreshTokens.Add(newRefreshToken);

            var accessToken = await CreateAccessTokenAsync(
                storedRefreshToken.User,
                accessTokenExpiresAtUtc);

            await _context.SaveChangesAsync(
                cancellationToken);

            return new TokenResult(
                accessToken,
                accessTokenExpiresAtUtc,
                newRawRefreshToken,
                refreshTokenExpiresAtUtc);
        }

        public async Task RevokeRefreshTokenAsync(
            string refreshToken,
            string? ipAddress,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                return;

            var tokenHash = ComputeSha256(refreshToken);

            var storedRefreshToken = await _context.RefreshTokens
                .SingleOrDefaultAsync(
                    x => x.TokenHash == tokenHash,
                    cancellationToken);

            if (storedRefreshToken is null)
                return;

            storedRefreshToken.Revoke(ipAddress);

            await _context.SaveChangesAsync(
                cancellationToken);
        }

        private async Task<TokenResult> CreateTokensForUserAsync(
            AppUser user,
            string? ipAddress,
            CancellationToken cancellationToken)
        {
            var accessTokenExpiresAtUtc =
                DateTimeOffset.UtcNow.AddMinutes(
                    _jwtOptions.AccessTokenMinutes);

            var refreshTokenExpiresAtUtc =
                DateTimeOffset.UtcNow.AddDays(
                    _jwtOptions.RefreshTokenDays);

            var accessToken = await CreateAccessTokenAsync(
                user,
                accessTokenExpiresAtUtc);

            var rawRefreshToken = GenerateSecureToken();

            var refreshToken = new RefreshToken(
                ComputeSha256(rawRefreshToken),
                user.Id,
                refreshTokenExpiresAtUtc,
                ipAddress);

            _context.RefreshTokens.Add(refreshToken);

            await _context.SaveChangesAsync(
                cancellationToken);

            return new TokenResult(
                accessToken,
                accessTokenExpiresAtUtc,
                rawRefreshToken,
                refreshTokenExpiresAtUtc);
        }

        private async Task<string> CreateAccessTokenAsync(
            AppUser user,
            DateTimeOffset expiresAtUtc)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
        {
            new(
                JwtRegisteredClaimNames.Sub,
                user.Id.ToString()),

            new(
                ClaimTypes.NameIdentifier,
                user.Id.ToString()),

            new(
                ClaimTypes.Name,
                user.UserName ?? string.Empty),

            new(
                ClaimTypes.Email,
                user.Email ?? string.Empty)
        };

            foreach (var role in roles)
            {
                claims.Add(
                    new Claim(
                        ClaimTypes.Role,
                        role));
            }

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtOptions.Key));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expiresAtUtc.UtcDateTime,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }

        private static string GenerateSecureToken()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);

            return Convert.ToBase64String(randomBytes);
        }

        private static string ComputeSha256(string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);

            var hashBytes = SHA256.HashData(bytes);

            return Convert.ToHexString(hashBytes);
        }
    }
}
