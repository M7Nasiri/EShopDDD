using EShop.Shared.Application;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.PaymentAgg.Commands.ProcessPayment
{
    public sealed record ProcessPaymentCallbackCommand(
    Guid PaymentId,
    string Authority,
    string Status) : IBaseCommand<PaymentCallbackResult>;
}
