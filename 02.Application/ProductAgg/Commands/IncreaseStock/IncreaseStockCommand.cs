using EShop.Shared.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.ProductAgg.Commands.IncreaseStock
{
    public sealed record IncreaseStockCommand(Guid ProductId, int Quantity) :IBaseCommand;
}
