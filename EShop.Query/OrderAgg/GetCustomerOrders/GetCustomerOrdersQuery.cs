using EShop.Query.OrderAgg.DTOs;
using EShop.Shared.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.OrderAgg.GetCustomerOrders
{
    public class GetCustomerOrdersQuery : QueryFilter<CustomerOrdersFilterResult, CustomerOrdersFilterParams>
    {
        public GetCustomerOrdersQuery(CustomerOrdersFilterParams filterParams) : base(filterParams)
        {
        }
    }
}
