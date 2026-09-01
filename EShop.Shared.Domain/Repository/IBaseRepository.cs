using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EShop.Shared.Domain.Repository
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<T?> GetAsync(Guid id,CancellationToken token);

        Task<T?> GetTracking(Guid id, CancellationToken token);

        Task AddAsync(T entity, CancellationToken token);
        void Add(T entity);

        Task AddRange(ICollection<T> entities, CancellationToken token);

        void Update(T entity);

        Task<int> Save();

        Task<bool> ExistsAsync(Expression<Func<T, bool>> expression, CancellationToken token);
        Task<bool> ExistsAsync(Guid id, CancellationToken token);

        bool Exists(Expression<Func<T, bool>> expression);

        T? Get(Guid id);
    }
}
