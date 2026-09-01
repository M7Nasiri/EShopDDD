using EShop.Shared.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.ProductAgg.Commands.CreateProduct
{
    public sealed record CreateProductCommand(string Name,string Description,int Stock,decimal UnitPrice,
        Guid CategoryId) : IBaseCommand;
}
