using EShop.Query.CategoryAgg.DTOs;
using EShop.Shared.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.CategoryAgg.GetChildCategories
{
    public record GetChildCategoriesByParentIdQuery(Guid? ParentId) : IQuery<List<CategoryDto>>;
}
