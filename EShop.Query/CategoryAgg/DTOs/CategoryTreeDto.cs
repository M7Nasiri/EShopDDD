using EShop.Shared.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.CategoryAgg.DTOs
{
    public class CategoryTreeDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public Guid? ParentCategoryId { get; set; }
        public List<CategoryTreeDto> Children { get; set; } = new();
    }
}
