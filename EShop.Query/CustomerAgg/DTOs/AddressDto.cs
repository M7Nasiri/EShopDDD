namespace EShop.Query.CustomerAgg.DTOs;

public sealed record AddressDto(
    string Title,
    string ReceiverName,
    string PhoneNumber,
    string Province,
    string City,
    string Street,
    string Plaque,
    string PostalCode,
    bool IsDefault
);