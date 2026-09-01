using _01.Domain.Entities.Aggregates.CartAgg;
using _01.Domain.ValueObjects;
using EShop.Shared.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Entities.Aggregates.CategoryAgg.Repository
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<bool> HasChildrenAsync(
            Guid categoryId,
            CancellationToken cancellationToken = default);

       
    }
}
