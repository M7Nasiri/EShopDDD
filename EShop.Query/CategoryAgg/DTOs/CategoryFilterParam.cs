using EShop.Shared.Query.Filter;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.CategoryAgg.DTOs
{
    public class CategoryFilterParam : BaseFilterParam
    {
        public string? Search { get; set; }
        public Guid? ParentId { get; set; }
    }
}
