using System;
using System.Collections.Generic;
using System.Text;
using EShop.Shared.Query;

namespace EShop.Query.PaymentAgg.DTOs.Customer
{
    public class CustomerPaymentsDto : BaseDto
    {
        public Guid PaymentId { get; init; }
        public decimal Amount { get; init; }
        public string Status { get; init; }
        public string Method  { get; init; }
        public string? GatewayTransactionId { get; init; }
        public string? RefundTransactionId { get; init; }
    }
        
}
