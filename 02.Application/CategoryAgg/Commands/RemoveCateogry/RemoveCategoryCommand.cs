using EShop.Shared.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.CategoryAgg.Commands.RemoveCateogry
{
    public sealed record RemoveCategoryCommand(
     Guid CategoryId) : IBaseCommand;
}
