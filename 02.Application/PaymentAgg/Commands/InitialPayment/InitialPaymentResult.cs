using EShop.Shared.Application.Interfaces.ExternalServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.PaymentAgg.Commands.InitialPayment
{
    public sealed record InitiatePaymentResult(
    Guid PaymentId,
    string RedirectUrl,
    string Authority);
}
