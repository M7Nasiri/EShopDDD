using _01.Domain.Consts;
using EShop.Shared.Query.Filter;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.PaymentAgg.DTOs.Customer
{
    public class CustomerPaymentFilterParams : BaseFilterParam
    {
        public string? Search { get; set; }
        public PaymentStatus? Status { get; set; }
    }
}
