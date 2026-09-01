using EShop.Shared.Application;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.PaymentAgg.Commands.RefundPayment
{
    public sealed record RefundPaymentCommand(
    Guid PaymentId,
    string Reason) : IBaseCommand;
}
