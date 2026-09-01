using EShop.Shared.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.CartAgg.Commands.AddItem
{
    public record AddItemCommand(Guid ProductId, int Quantity) : IBaseCommand;

}
