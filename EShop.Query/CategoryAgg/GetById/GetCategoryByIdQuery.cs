using EShop.Query.CategoryAgg.DTOs;
using EShop.Shared.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.CategoryAgg.GetById
{
    public record GetCategoryByIdQuery(Guid CategoryId) : IQuery<CategoryDto>;
}
