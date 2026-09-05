using EShop.Infrastructure.PersistentEFCore;
using EShop.Query.CategoryAgg.DTOs;
using EShop.Shared.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.CategoryAgg.GetById
{
    public class GetCategoryByIdQueryHandler : IQueryHandler<GetCategoryByIdQuery, CategoryDto>
    {
        private readonly ShopContext _context;
        public GetCategoryByIdQueryHandler(ShopContext context)
        {
            _context = context;
        }
        public async Task<CategoryDto?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var model = await _context.Categories.FirstOrDefaultAsync(c => c.Id == request.CategoryId);
            return model.Map();

        }
    }
}
