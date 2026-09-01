using _01.Domain.Consts;
using _01.Domain.Entities.Aggregates.CommentAgg;
using _01.Domain.Entities.Aggregates.CommentAgg.Repository;
using _01.Domain.ValueObjects;
using EShop.Infrastructure._Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Infrastructure.PersistentEFCore.CommentAgg
{
    public class CommentRepository : BaseRepository<ProductComment>, ICommentRepository
    {
        private readonly ShopContext _context;

        public CommentRepository(ShopContext context) :base(context)
        {
            _context = context;
        }

        public Task<bool> ExistsByProductAndCustomerAsync(
           _01.Domain.Guid productId,
           _01.Domain.Guid customerId,
           CancellationToken cancellationToken = default)
        {
            return _context.Set<ProductComment>()
                .AnyAsync(
                    x => x.ProductId.Value == productId.Value &&
                         x.CustomerId.Value == customerId.Value,
                    cancellationToken);
        }

        public async Task<IReadOnlyList<ProductComment>>
            GetApprovedByProductIdAsync(
                System.Guid productId,
                CancellationToken cancellationToken = default)
        {
            return await _context.Set<ProductComment>()
                .AsNoTracking()
                .Where(x =>
                    x.ProductId.Value == productId &&
                    x.Status == ProductCommentStatus.Approved)
                .OrderByDescending(x => x.Id.Value)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<ProductComment>>
            GetPendingAsync(
                CancellationToken cancellationToken = default)
        {
            return await _context.Set<ProductComment>()
                .Where(x => x.Status == ProductCommentStatus.Pending)
                .OrderBy(x => x.Id.Value)
                .ToListAsync(cancellationToken);
        }

        public void Remove(ProductComment comment)
        {
            _context.Set<ProductComment>().Remove(comment);
        }
    }
}
