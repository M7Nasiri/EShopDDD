using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Infrastructure.Identity.Entities;
public sealed class RefreshToken
{
    public Guid Id { get; private set; }

    // Hash خود Refresh Token، نه مقدار خام آن
    public string TokenHash { get; private set; }

    public DateTimeOffset ExpiresAtUtc { get; private set; }

    public DateTimeOffset CreatedAtUtc { get; private set; }

    public DateTimeOffset? RevokedAtUtc { get; private set; }

    // برای Token Rotation:
    // وقتی یک Refresh Token استفاده می‌شود، توکن قبلی revoke
    // و توکن جدید ساخته می‌شود.
    public string? ReplacedByTokenHash { get; private set; }

    public string? CreatedByIp { get; private set; }

    public string? RevokedByIp { get; private set; }

    public Guid UserId { get; private set; }

    public AppUser User { get; private set; } = null!;

    private RefreshToken()
    {
        // For EF Core
    }

    public RefreshToken(
        string tokenHash,
        Guid userId,
        DateTimeOffset expiresAtUtc,
        string? createdByIp = null)
    {
        if (string.IsNullOrWhiteSpace(tokenHash))
            throw new ArgumentException(
                "Token hash is required.",
                nameof(tokenHash));

        if (userId == Guid.Empty)
            throw new ArgumentException(
                "User id is required.",
                nameof(userId));

        if (expiresAtUtc <= DateTimeOffset.UtcNow)
            throw new ArgumentException(
                "Refresh token expiration must be in the future.",
                nameof(expiresAtUtc));

        Id = Guid.NewGuid();;
        TokenHash = tokenHash;
        UserId = userId;
        ExpiresAtUtc = expiresAtUtc;
        CreatedAtUtc = DateTimeOffset.UtcNow;
        CreatedByIp = createdByIp;
    }

    public bool IsExpired =>
        DateTimeOffset.UtcNow >= ExpiresAtUtc;

    public bool IsRevoked =>
        RevokedAtUtc.HasValue;

    public bool IsActive =>
        !IsRevoked && !IsExpired;

    public void Revoke(
        string? revokedByIp = null,
        string? replacedByTokenHash = null)
    {
        if (IsRevoked)
            return;

        RevokedAtUtc = DateTimeOffset.UtcNow;
        RevokedByIp = revokedByIp;
        ReplacedByTokenHash = replacedByTokenHash;
    }
}