using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Abstractions.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Entities.Aggregates.MembershipPlanAgg
{
    public class MembershipPlan : AggregateRoot
    {
        public Id Id { get; private set; }

        public string Name { get; private set; }

        public Money Price { get; private set; }

        public MembershipDuration Duration { get; private set; }

        public int DiscountPercent { get; private set; }

        public bool IsActive { get; private set; }

        private MembershipPlan()
        {
        }

        public MembershipPlan(
            Id id,
            string name,
            Money price,
            MembershipDuration duration,
            int discountPercent)
        {
            if (id is null)
                throw new EShopNullException(nameof(id));

            if (string.IsNullOrWhiteSpace(name))
                throw new EShopDomainException(
                    "Membership plan name is required.");

            if (price is null)
                throw new EShopNullException(nameof(price));

            if (duration is null)
                throw new EShopNullException(nameof(duration));

            if (discountPercent is < 0 or > 100)
                throw new EShopDomainException(
                    "Discount percent must be between 0 and 100.");

            Id = id;
            Name = name.Trim();
            Price = price;
            Duration = duration;
            DiscountPercent = discountPercent;
            IsActive = true;
        }

        public void ChangePrice(Money price)
        {
            ArgumentNullException.ThrowIfNull(price);

            Price = price;
        }

        public void ChangeDiscount(int discountPercent)
        {
            if (discountPercent is < 0 or > 100)
                throw new EShopDomainException(
                    "Discount percent must be between 0 and 100.");

            DiscountPercent = discountPercent;
        }

        public void ChangeDuration(MembershipDuration duration)
        {
            ArgumentNullException.ThrowIfNull(duration);

            Duration = duration;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }
    }
}
