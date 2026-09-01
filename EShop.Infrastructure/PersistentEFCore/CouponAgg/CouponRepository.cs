using _01.Domain.Entities.Aggregates.CouponAgg;
using _01.Domain.Entities.Aggregates.CouponAgg.Repository;
using EShop.Infrastructure._Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Infrastructure.PersistentEFCore.CouponAgg
{
    public class CouponRepository : BaseRepository<Coupon>, ICouponRepository
    {
        private readonly ShopContext _context;

        public CouponRepository(ShopContext context) : base(context)
        {
            _context = context;
        }
      
        public async Task<Coupon?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
        {
            var normalizedCode = code.Trim().ToUpperInvariant();
            return await _context.Set<Coupon>()
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Code == normalizedCode, cancellationToken);
        }

        public async Task<Coupon?> GetByCodeTrackingAsync(string code, CancellationToken cancellationToken = default)
        {
            var normalizedCode = code.Trim().ToUpperInvariant();
            return await _context.Set<Coupon>()
                .AsTracking()
                .FirstOrDefaultAsync(c => c.Code == normalizedCode, cancellationToken);
        }

        public async Task<bool> IsCodeUniqueAsync(string code, CancellationToken cancellationToken = default)
        {
            var normalizedCode = code.Trim().ToUpperInvariant();
            return !await _context.Set<Coupon>()
                .AnyAsync(c => c.Code == normalizedCode, cancellationToken);
        }
    }
}
