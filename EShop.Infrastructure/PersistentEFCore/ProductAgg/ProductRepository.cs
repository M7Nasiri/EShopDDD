using _01.Domain.Entities.Aggregates.ProductAgg;
using _01.Domain.Entities.Aggregates.ProductAgg.Repository;
using EShop.Infrastructure._Utilities;
using EShop.Shared.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Infrastructure.PersistentEFCore.ProductAgg
{
    internal class ProductRepository : BaseRepository<Product> , IProductRepository
    {
        private readonly ShopContext _dbContext;
        public ProductRepository(ShopContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<bool> ExistsByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken = default)
        {
            return _dbContext.Set<Product>().AnyAsync(x => x.CategoryId.Value == categoryId,
                   cancellationToken);
        }
      
    }
}
