using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Shared.Application.Interfaces.Authentication
{
    public interface ITokenService
    {
        Task<TokenResult> CreateTokensAsync(
            Guid userId,
            string? ipAddress,
            CancellationToken cancellationToken = default);

        Task<TokenResult> RefreshTokensAsync(
            string refreshToken,
            string? ipAddress,
            CancellationToken cancellationToken = default);

        Task RevokeRefreshTokenAsync(
            string refreshToken,
            string? ipAddress,
            CancellationToken cancellationToken = default);
    }
}
