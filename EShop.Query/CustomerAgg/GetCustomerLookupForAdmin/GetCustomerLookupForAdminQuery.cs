using EShop.Query.CustomerAgg.DTOs;
using EShop.Shared.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.CustomerAgg.GetCustomerLookupForAdmin
{
    public class GetCustomerLookupForAdminQuery : QueryFilter<CustomerFilterResult,CustomerFilterParams>
    {
        public GetCustomerLookupForAdminQuery(CustomerFilterParams filterParams) : base(filterParams)
        {
        }
    }
}
