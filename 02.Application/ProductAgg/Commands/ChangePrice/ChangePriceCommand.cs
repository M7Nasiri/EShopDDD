using EShop.Shared.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.ProductAgg.Commands.ChangePrice
{
    public sealed record ChangePriceCommand(Guid ProductId, decimal NewPrice) : IBaseCommand;
}
