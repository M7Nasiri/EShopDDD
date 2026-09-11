using EShop.Query.CouponAgg.DTOs;
using EShop.Shared.Query;
using System;
using System.Collections.Generic;
using System.Text;
using EShop.Infrastructure.PersistentEFCore;
using Microsoft.EntityFrameworkCore;

namespace EShop.Query.CouponAgg.ValidateCoupon
{
    public class ValidateCouponQueryHandler(ShopContext dbContext)
       : IQueryHandler<ValidateCouponQuery, ValidateCouponDto>
    {
        public async Task<ValidateCouponDto> Handle(ValidateCouponQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Code))
                return ValidateCouponDto.Invalid("کد تخفیف نمی‌تواند خالی باشد.");

            var normalizedCode = request.Code.Trim().ToUpperInvariant();
            var now = DateTime.UtcNow;

            var coupon = await dbContext.Coupons
                .AsNoTracking()
                .Where(c => c.Code == normalizedCode)
                .Select(c => new
                {
                    c.Id,
                    c.Code,
                    c.Percent,
                    c.IsActive,
                    StartDate = c.StartDate.Value, 
                    EndDate = c.EndDate.Value,
                    c.UsageLimit,
                    c.UsedCount
                })
                .FirstOrDefaultAsync(cancellationToken);


            if (coupon is null)
                return ValidateCouponDto.Invalid("کد تخفیف وارد شده معتبر نیست یا یافت نشد.");


            if (!coupon.IsActive)
                return ValidateCouponDto.Invalid("این کد تخفیف در حال حاضر غیرفعال است.");


            if (now < coupon.StartDate)
                return ValidateCouponDto.Invalid("مهلت استفاده از این کد تخفیف هنوز شروع نشده است.");

            if (now >= coupon.EndDate)
                return ValidateCouponDto.Invalid("مهلت استفاده از این کد تخفیف به پایان رسیده است.");


            if (coupon.UsageLimit.HasValue && coupon.UsedCount >= coupon.UsageLimit.Value)
                return ValidateCouponDto.Invalid("ظرفیت استفاده از این کد تخفیف تکمیل شده است.");

            return ValidateCouponDto.Valid(coupon.Id, coupon.Code, coupon.Percent);
        }
    }
}
