using System;
using System.Collections.Generic;
using System.Text;
using EShop.Query.PaymentAgg.DTOs.Admin;
using EShop.Shared.Query.Filter;

namespace EShop.Query.PaymentAgg.DTOs.Customer
{
    public class CustomerPaymentFilterResult :
        BaseFilter<CustomerPaymentsDto, CustomerPaymentFilterParams>;
}
