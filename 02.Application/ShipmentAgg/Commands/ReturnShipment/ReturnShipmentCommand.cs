using EShop.Shared.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.ShipmentAgg.Commands.ReturnShipment
{
    public sealed record ReturnShipmentCommand(Guid ShipmentId) : IBaseCommand;
}
