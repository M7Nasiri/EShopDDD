using EShop.Shared.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.OrderAgg.Commands.RemoveItemFromOrder
{
    public sealed record RemoveItemFromOrderCommand(Guid ProductId) : IBaseCommand;
}
