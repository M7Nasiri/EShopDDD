using EShop.Infrastructure.PersistentEFCore;
using EShop.Query.CategoryAgg.DTOs;
using EShop.Shared.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.CategoryAgg.GetCategoriesByFilter
{
    public class GetCategoriesByFilterQueryHandler : IQueryHandler<GetCategoriesByFilterQuery, CategoryFilterResult>
    {
        private readonly ShopContext _context;

        public GetCategoriesByFilterQueryHandler(ShopContext context)
        {
            _context = context;
        }

        public async Task<CategoryFilterResult> Handle(GetCategoriesByFilterQuery request, CancellationToken cancellationToken)
        {
            var @params = request.FilterParams;
            var query = _context.Categories.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(@params.Search))
            {
                query = query.Where(c => c.Name.Value.Contains(@params.Search));
            }

            if (@params.ParentId.HasValue)
            {
                query = query.Where(c => c.ParentCategoryId == @params.ParentId.Value);
            }

            var result = new CategoryFilterResult
            {
                FilterParams = @params
            };


            var count = await query.CountAsync(cancellationToken);
            result.GeneratePaging(count, @params.Take, @params.PageId);


            var skip = (@params.PageId - 1) * @params.Take;
            result.Data = await query
                .OrderByDescending(c => c.Id)
                .Skip(skip)
                .Take(@params.Take)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name.Value,
                    ParentCategoryId = c.ParentCategoryId
                })
                .ToListAsync(cancellationToken);

            return result;
        }
    }
}
