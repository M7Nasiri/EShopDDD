using EShop.Shared.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.ShipmentAgg.Commands.ShipOrder
{
    public sealed record ShipOrderCommand(Guid ShipmentId, string TrackingCode) : IBaseCommand;
}
