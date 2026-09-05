using _01.Domain.ValueObjects;
using EShop.Shared.Query;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace EShop.Query.CategoryAgg.DTOs
{
    public class CategoryDto : BaseDto
    {
        public string Name { get;  set; }

        public Guid? ParentCategoryId { get;  set; }
        public string? ParentName { get; set; }
    }
}
