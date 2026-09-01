namespace EShop.WebAPI.DTOs.Auth
{
    public sealed record LoginRequest(
    string UserNameOrEmail,
    string Password);
}
