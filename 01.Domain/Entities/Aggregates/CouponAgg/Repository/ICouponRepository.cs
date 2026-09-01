using EShop.Shared.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Entities.Aggregates.CouponAgg.Repository
{
    public interface ICouponRepository : IBaseRepository<Coupon>
    {

        Task<Coupon?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);

        Task<Coupon?> GetByCodeTrackingAsync(string code, CancellationToken cancellationToken = default);

        Task<bool> IsCodeUniqueAsync(string code, CancellationToken cancellationToken = default);
    }
}
