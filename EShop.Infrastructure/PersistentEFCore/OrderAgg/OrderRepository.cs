using _01.Domain.Consts;
using _01.Domain.Entities.Aggregates.OrderAgg;
using _01.Domain.Entities.Aggregates.OrderAgg.Repository;
using _01.Domain.ValueObjects;
using EShop.Infrastructure._Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Infrastructure.PersistentEFCore.OrderAgg
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        private readonly ShopContext _context;

        public OrderRepository(ShopContext context) : base(context) 
        {
            _context = context;
        }

        public async Task<Order?> GetByIdReadOnlyAsync(Guid orderId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Order>()
            .AsNoTracking()
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
        }

        public async Task<Order?> GetDraftOrderByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Order>()
             .Include(o => o.Items).AsTracking()
             .FirstOrDefaultAsync(o => o.CustomerId == customerId && o.Status == OrderStatus.Draft, cancellationToken);
        }

        public async Task<Order?> GetWithItemsTrackingAsync(Guid orderId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Order>().Include(o=>o.Items).Where(o=>o.Id == orderId)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
