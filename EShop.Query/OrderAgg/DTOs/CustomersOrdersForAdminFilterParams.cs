using _01.Domain.Consts;
using EShop.Shared.Query.Filter;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.OrderAgg.DTOs
{
    public class CustomersOrdersForAdminFilterParams : BaseFilterParam
    {
        public Guid? CustomerId { get; set; }
        public string? Search { get; set; }
        public OrderStatus? OrderStatus { get; set; } // وضعیت سفارش در صورت فیلتر
    }
}
