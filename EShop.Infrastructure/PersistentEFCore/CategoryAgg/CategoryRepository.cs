using _01.Domain.Entities.Aggregates.CategoryAgg;
using _01.Domain.Entities.Aggregates.CategoryAgg.Repository;
using _01.Domain.ValueObjects;
using EShop.Infrastructure._Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Infrastructure.PersistentEFCore.CategoryAgg
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        private readonly ShopContext _dbContext;
        public CategoryRepository(ShopContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public Task<bool> HasChildrenAsync(Guid categoryId, CancellationToken cancellationToken = default)
        {
            return _dbContext.Set<Category>()
            .AnyAsync(
                x => x.ParentCategoryId == categoryId,
                cancellationToken);
        }
    }
}
