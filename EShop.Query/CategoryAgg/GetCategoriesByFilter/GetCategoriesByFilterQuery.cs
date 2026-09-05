using EShop.Query.CategoryAgg.DTOs;
using EShop.Shared.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.CategoryAgg.GetCategoriesByFilter
{
    public class GetCategoriesByFilterQuery : QueryFilter<CategoryFilterResult, CategoryFilterParam>
    {
        public GetCategoriesByFilterQuery(CategoryFilterParam filterParams) : base(filterParams)
        {
        }
    }
}
