using EShop.Infrastructure.PersistentEFCore;
using EShop.Shared.Domain;
using EShop.Shared.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EShop.Infrastructure._Utilities
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly ShopContext Context;
        public BaseRepository(ShopContext context)
        {
            Context = context;
        }

        void IBaseRepository<TEntity>.Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }
      
        public void Update(TEntity entity)
        {
            Context.Update(entity);
        }
        public async Task<int> Save()
        {
            return await Context.SaveChangesAsync();
        }

        public bool Exists(Expression<Func<TEntity, bool>> expression)
        {
            return Context.Set<TEntity>().Any(expression);
        }

        public async virtual Task<TEntity?> GetAsync(Guid id, CancellationToken token)
        {
            return await Context.Set<TEntity>().FirstOrDefaultAsync(t => t.Id.Equals(id));
        }

        public async Task<TEntity?> GetTracking(Guid id, CancellationToken token)
        {
            return await Context.Set<TEntity>().AsTracking().FirstOrDefaultAsync(t => t.Id.Equals(id));
        }

        public async Task AddAsync(TEntity entity, CancellationToken token)
        {
            await Context.Set<TEntity>().AddAsync(entity);
        }

        public async Task AddRange(ICollection<TEntity> entities, CancellationToken token)
        {
            await Context.Set<TEntity>().AddRangeAsync(entities);
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression, CancellationToken token)
        {
            return await Context.Set<TEntity>().AnyAsync(expression);
        }
        public async Task<bool> ExistsAsync(Guid id, CancellationToken token)
        {
            return await Context.Set<TEntity>().AnyAsync(x=>x.Id.Equals(id));
        }

        public TEntity? Get(Guid id)
        {
            return Context.Set<TEntity>().FirstOrDefault(t => t.Id.Equals(id));
        }
        public Task<int> RemoveAsync(Guid id,CancellationToken token=default)
        {
            return Context.Set<TEntity>().Where(e=>e.Id.Equals(id) && !e.IsDelete).ExecuteUpdateAsync(setters=>setters
            .SetProperty(e=>e.IsDelete,true),token);
        }
    }
}
