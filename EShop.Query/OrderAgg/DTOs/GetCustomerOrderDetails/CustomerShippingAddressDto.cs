namespace EShop.Query.OrderAgg.DTOs.GetCustomerOrderDetails;

public record CustomerShippingAddressDto(
    string ReceiverName,
    string PhoneNumber,
    string Province,
    string City,
    string Street,
    string Plaque,
    string PostalCode);