using EShop.Shared.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.CategoryAgg.Commands.AddCategory
{
    public sealed record AddCategoryCommand(
    string Name,
    Guid? ParentCategoryId = null) : IBaseCommand;
}
