namespace EShop.Query.CustomerAgg.DTOs;

public sealed class CustomerDetailsForAdminDto
{
    public Guid Id { get; init; }
    public string FullName { get; init; } = default!;
    public string? DefaultPostalCode { get; init; }
    public List<AddressDto> Addresses { get; init; } = new();
}