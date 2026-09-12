using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.PaymentAgg.DTOs
{
    public record PaymentDetailDto(
        Guid PaymentId,
        Guid OrderId,
        decimal Amount,
        string Method,
        string Status,
        string? GatewayTransactionId,
        string? RefundTransactionId,
        DateTime CreatedAt,
        string? Authority);
}
