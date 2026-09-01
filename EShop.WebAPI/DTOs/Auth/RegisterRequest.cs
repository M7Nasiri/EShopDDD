namespace EShop.WebAPI.DTOs.Auth
{
    public sealed record RegisterRequest(
    string UserName,
    string Email,
    string Password,
    string? FirstName,
    string? LastName);
}
