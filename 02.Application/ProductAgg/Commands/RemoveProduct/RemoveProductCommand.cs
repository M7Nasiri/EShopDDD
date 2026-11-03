using System;
using System.Collections.Generic;
using System.Text;
using EShop.Shared.Application;

namespace _02.Application.ProductAgg.Commands.RemoveProduct
{
    public record RemoveProductCommand(Guid ProductId) : IBaseCommand;
}
