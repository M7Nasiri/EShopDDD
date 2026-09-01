using EShop.Shared.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.OrderAgg.Commands.ProcessOrderPaymentCallback
{
    public sealed record ProcessOrderPaymentCallbackCommand(Guid OrderId,string TrackingNumber) : IBaseCommand;
}
