using EShop.Query.CouponAgg.DTOs;
using EShop.Shared.Query;
using System;
using System.Collections.Generic;
using System.Text;
using _01.Domain.Exceptions;
using EShop.Infrastructure.PersistentEFCore;
using Microsoft.EntityFrameworkCore;

namespace EShop.Query.CouponAgg.GetCouponByCode
{
    public class GetCouponByCodeQueryHandler(ShopContext dbContext) : IQueryHandler<GetCouponByCodeQuery, CouponDetailsDto?>
    {
        public async Task<CouponDetailsDto?> Handle(GetCouponByCodeQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Code))
                throw new EShopDomainException("کد تخفیف نمی‌تواند خالی باشد.");

            var normalizedCode = request.Code.Trim().ToUpperInvariant();
            var couponDto = await dbContext.Coupons.AsNoTracking().Where(c => c.Code == normalizedCode)
                .Select(c => new CouponDetailsDto()
                {
                    Code = c.Code,
                    CreatedByUserId = c.CreatedByUserId,
                    StartDate = c.StartDate.Value,
                    EndDate = c.EndDate.Value,
                    IsActive = c.IsActive,
                    Percent = c.Percent,
                    UsageLimit = c.UsageLimit,
                    UsedCount = c.UsedCount
                }).FirstOrDefaultAsync(cancellationToken);
            return couponDto;
        }
    }
}
