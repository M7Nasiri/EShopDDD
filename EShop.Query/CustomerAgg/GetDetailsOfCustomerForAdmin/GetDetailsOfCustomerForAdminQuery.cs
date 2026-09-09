using EShop.Query.CustomerAgg.DTOs;
using EShop.Shared.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.CustomerAgg.GetDetailsOfCustomerForAdmin
{
    public sealed record GetDetailsOfCustomerForAdminQuery(Guid CustomerId)
        : IQuery<CustomerDetailsForAdminDto?>;
}
