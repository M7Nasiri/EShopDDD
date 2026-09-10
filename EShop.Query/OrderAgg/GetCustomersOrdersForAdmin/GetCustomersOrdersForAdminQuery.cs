using System;
using System.Collections.Generic;
using System.Text;
using EShop.Query.OrderAgg.DTOs;
using EShop.Shared.Query;

namespace EShop.Query.OrderAgg.GetCustomersOrdersForAdmin
{
    public class GetCustomersOrdersForAdminQuery : QueryFilter<CustomersOrdersForAdminFilterResult, CustomersOrdersForAdminFilterParams>
    {
        public GetCustomersOrdersForAdminQuery(CustomersOrdersForAdminFilterParams filterParams) : base(filterParams)
        {
        }
    }
}
