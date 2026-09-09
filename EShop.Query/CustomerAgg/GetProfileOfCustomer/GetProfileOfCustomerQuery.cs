using System;
using System.Collections.Generic;
using System.Text;
using EShop.Query.CustomerAgg.DTOs;
using EShop.Shared.Query;

namespace EShop.Query.CustomerAgg.GetProfileOfCustomer
{
    public record GetProfileOfCustomerQuery(Guid CustomerId) : IQuery<CustomerProfileDto>;

}
