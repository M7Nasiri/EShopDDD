using EShop.Shared.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.OrderAgg.Commands.AddItemToOrder
{
    public sealed record AddItemToOrderCommand(Guid ProductId, int Quantity) : IBaseCommand;

}
