using System;
using System.Collections.Generic;
using System.Text;
using EShop.Shared.Query;

namespace EShop.Query.CustomerAgg.DTOs
{
    public sealed record CustomerProfileDto(
        Guid Id,
        string FullName,
        AddressDto? DefaultAddress
    ) ;

}
