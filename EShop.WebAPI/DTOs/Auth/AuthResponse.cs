namespace EShop.WebAPI.DTOs.Auth
{
    public sealed record AuthResponse(
    string AccessToken,
    DateTimeOffset AccessTokenExpiresAtUtc);
}
