using EShop.Shared.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.CategoryAgg.Commands.EditCategory
{
    public sealed record EditCategoryCommand(
     Guid CategoryId,
     string Name,
     Guid? ParentCategoryId) : IBaseCommand;
}
