using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace EShop.Shared.Application.Interfaces.Events
{
    public sealed record PaymentSuccededIntegrationEvent(Guid PaymentId,Guid OrderId, decimal Amount,DateTime OccuredOn);
}
