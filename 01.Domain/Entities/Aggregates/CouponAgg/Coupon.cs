using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Entities.Aggregates.CouponAgg
{
    public sealed class Coupon : AggregateRoot
    {
        public Guid Id { get; private set; }

        public string Code { get; private set; }

        public int Percent { get; private set; }

        public Guid CreatedByUserId { get; private set; }

        public DomainDate StartDate { get; private set; }

        public DomainDate EndDate { get; private set; }

        public bool IsActive { get; private set; }

        public int? UsageLimit { get; private set; }

        public int UsedCount { get; private set; }

        private Coupon()
        {
        }

        public Coupon(
            string code,
            int percent,
            Guid createdByUserId,
            DomainDate startDate,
            DomainDate endDate,
            int? usageLimit = null)
        {
            ArgumentNullException.ThrowIfNull(startDate);
            ArgumentNullException.ThrowIfNull(endDate);

            if (string.IsNullOrWhiteSpace(code))
                throw new EShopCouponException();

            if (percent is < 1 or > 100)
                throw new EShopCouponException();

            if (startDate >= endDate)
                throw new EShopDomainException(
                    "Coupon start date must be before end date.");

            if (usageLimit is <= 0)
                throw new EShopDomainException(
                    "Usage limit must be greater than zero.");

            Id = Guid.NewGuid();
            Code = code.Trim().ToUpperInvariant();
            Percent = percent;
            CreatedByUserId = createdByUserId;
            StartDate = startDate;
            EndDate = endDate;
            UsageLimit = usageLimit;
            IsActive = true;
        }

        public bool IsValidAt(DomainDate date)
        {
            ArgumentNullException.ThrowIfNull(date);

            if (!IsActive)
                return false;

            if (date < StartDate || date >= EndDate)
                return false;

            if (UsageLimit.HasValue &&
                UsedCount >= UsageLimit.Value)
                return false;

            return true;
        }

        public bool IsValidNow() =>
            IsValidAt(DomainDate.Now);

        public void Disable()
        {
            IsActive = false;
        }

        public void Enable()
        {
            IsActive = true;
        }

        public void RecordUsage()
        {
            if (UsageLimit.HasValue &&
                UsedCount >= UsageLimit.Value)
            {
                throw new EShopCouponException();
            }

            UsedCount++;
        }

        public AppliedCouponSnapshot CreateSnapshot()
        {
            if (!IsValidNow())
                throw new EShopCouponException();

            return new AppliedCouponSnapshot(
                Code,
                Percent);
        }
    }
}
