using _01.Domain.Entities.Aggregates.CustomerAgg;
using EShop.Query.PaymentAgg.DTOs.Customer;
using EShop.Shared.Query;
using EShop.Shared.Query.Filter;
using MassTransit.Caching;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.PaymentAgg.GetCustomerPayments
{
    public class GetCustomerPaymentsQuery(Guid customerId, CustomerPaymentFilterParams filterParams) :
        QueryFilter<CustomerPaymentFilterResult, CustomerPaymentFilterParams>(filterParams)
    {
        public Guid CustomerId { get; } = customerId;
    }
}
