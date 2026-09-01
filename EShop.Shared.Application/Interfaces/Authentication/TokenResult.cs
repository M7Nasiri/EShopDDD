using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Shared.Application.Interfaces.Authentication
{
    public sealed record TokenResult(
    string AccessToken,
    DateTimeOffset AccessTokenExpiresAtUtc,
    string RefreshToken,
    DateTimeOffset RefreshTokenExpiresAtUtc);
}
