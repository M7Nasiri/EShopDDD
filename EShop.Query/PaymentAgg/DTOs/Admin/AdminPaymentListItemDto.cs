using System;
using System.Collections.Generic;
using System.Text;
using EShop.Shared.Query;

namespace EShop.Query.PaymentAgg.DTOs.Admin
{
    public sealed class AdminPaymentListItemDto : BaseDto
    {
        public Guid OrderId { get; init; }
        public string CustomerFullName { get; init; }
        public decimal Amount { get; init; }
        public string Method { get; init; }
        public string Status { get; init; }
        public string? GatewayTransactionId { get; init; }
        public string? RefundTransactionId { get; init; }
        
    }
       
}
