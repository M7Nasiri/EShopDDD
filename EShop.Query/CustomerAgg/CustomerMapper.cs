using System;
using System.Collections.Generic;
using System.Text;
using _01.Domain.ValueObjects;
using EShop.Query.CustomerAgg.DTOs;

namespace EShop.Query.CustomerAgg
{
    public static class CustomerMapper
    {
        public static AddressDto MapToAddressDto(this Address address)
        {
            if (address is null)
                return null;
            return new AddressDto(address.Title,
                address.ReceiverName, address.PhoneNumber, address.Province,
               address.City,address.Street, address.Plaque, address.PostalCode, true
            );
        }


    }
}
