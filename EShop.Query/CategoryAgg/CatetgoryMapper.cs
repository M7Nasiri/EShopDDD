using _01.Domain.Entities.Aggregates.CategoryAgg;
using EShop.Infrastructure.PersistentEFCore;
using EShop.Query.CategoryAgg.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.CategoryAgg
{
    public static class CatetgoryMapper
    {
        public static CategoryDto Map(this Category? category)
        {
            if (category == null)
                return null;

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name.Value,
                ParentCategoryId = category.ParentCategoryId,
            };
        }
    }
}
