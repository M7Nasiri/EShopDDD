using System;
using System.Collections.Generic;
using System.Text;
using EShop.Query.CustomerAgg.DTOs;
using EShop.Shared.Query;

namespace EShop.Query.CustomerAgg.GetAddressesOfCustomer
{
    public record GetAddressesOfCustomerQuery(Guid CustoemrId) : IQuery<IReadOnlyList<AddressDto>>;
}
