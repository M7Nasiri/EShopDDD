using EShop.Query.CustomerAgg.DTOs.Orders;
using EShop.Shared.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.CustomerAgg.GetCustomerOrders
{
    public class GetCustomerOrdersQuery : QueryFilter<CustomerOrdersFilterResult, CustomerOrdersFilterParams>
    {
        public GetCustomerOrdersQuery(CustomerOrdersFilterParams filterParams) : base(filterParams)
        {
        }
    }
}
