using EShop.Shared.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.ShipmentAgg.Commands.DeliverShip
{
    public sealed record DeliverShipmentCommand(Guid ShipmentId) : IBaseCommand;
}
