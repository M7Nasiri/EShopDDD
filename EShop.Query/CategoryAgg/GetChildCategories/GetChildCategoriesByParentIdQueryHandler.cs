using EShop.Infrastructure.PersistentEFCore;
using EShop.Query.CategoryAgg.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using EShop.Shared.Query;

namespace EShop.Query.CategoryAgg.GetChildCategories
{
    public class GetChildCategoriesByParentIdQueryHandler : IQueryHandler<GetChildCategoriesByParentIdQuery, List<CategoryDto>>
    {
        private readonly ShopContext _context;

        public GetChildCategoriesByParentIdQueryHandler(ShopContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryDto>> Handle(GetChildCategoriesByParentIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Categories
                .AsNoTracking()
                .Where(c => c.ParentCategoryId == request.ParentId)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name.Value,
                    ParentCategoryId = c.ParentCategoryId
                })
                .ToListAsync(cancellationToken);
        }
    }
}
