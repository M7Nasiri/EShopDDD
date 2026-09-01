using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.PaymentAgg.Commands.ProcessPayment
{
    public sealed record PaymentCallbackResult(
         bool IsSuccess,
         string? TransactionCode,
         string? ErrorMessage);
}
