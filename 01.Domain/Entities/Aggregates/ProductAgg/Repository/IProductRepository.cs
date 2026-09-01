using EShop.Shared.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Entities.Aggregates.ProductAgg.Repository
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<bool> ExistsByCategoryIdAsync(
           Guid categoryId,
           CancellationToken cancellationToken = default);
    }
}
