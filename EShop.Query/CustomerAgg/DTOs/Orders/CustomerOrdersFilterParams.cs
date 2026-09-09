using EShop.Shared.Query.Filter;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.CustomerAgg.DTOs.Orders
{
    public class CustomerOrdersFilterParams : BaseFilterParam
    {
        public Guid CustomerId { get; set; }
        public string? OrderStatus { get; set; } // وضعیت سفارش در صورت فیلتر
    }
}
