using System;
using System.Collections.Generic;
using System.Text;
using EShop.Query.OrderAgg.DTOs.GetCustomerOrderDetails;
using EShop.Shared.Query;

namespace EShop.Query.OrderAgg.GetCustomerOrderDetails
{
    public record GetCustomerOrderDetailsQuery(
        Guid OrderId,
        Guid CustomerId) : IQuery<CustomerOrderDetailDto?>;
}
