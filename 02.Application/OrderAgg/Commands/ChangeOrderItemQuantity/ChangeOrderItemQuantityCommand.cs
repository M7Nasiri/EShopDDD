using EShop.Shared.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.OrderAgg.Commands.ChangeOrderItemQuantity
{
    public sealed record ChangeOrderItemQuantityCommand(Guid ProductId,int NewQuantity) : IBaseCommand;
}
