using _01.Domain.Entities.Aggregates.CouponAgg;
using _01.Domain.Entities.Aggregates.CustomerAgg;
using _01.Domain.Entities.Aggregates.CustomerAgg.Repository;
using _01.Domain.ValueObjects;
using EShop.Infrastructure._Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Infrastructure.PersistentEFCore.CustomerAgg
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        private readonly ShopContext _context;

        public CustomerRepository(ShopContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Customer?> GetWithAddressesTrackingAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await Context.Set<Customer>()
                .Include(c => c.Addresses) 
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<Customer?> GetWithAddressesNoTrackingAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await Context.Set<Customer>()
                .AsNoTracking()
                .Include(c => c.Addresses)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }
    }
}
