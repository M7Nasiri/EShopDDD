using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.PaymentAgg.DTOs
{
    public record OrderPaymentStatusDto(Guid OrderId, string Status, bool IsPaid);
}
