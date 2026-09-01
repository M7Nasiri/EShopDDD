using _01.Domain.Consts;
using EShop.Shared.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.PaymentAgg.Commands.InitialPayment
{
    public sealed record InitiatePaymentCommand(Guid OrderId, PaymentMethod Method) : IBaseCommand<InitiatePaymentResult>;
}
