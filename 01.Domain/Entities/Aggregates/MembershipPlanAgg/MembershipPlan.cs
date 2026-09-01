using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Domain;

namespace _01.Domain.Entities.Aggregates.MembershipPlanAgg
{
    public sealed class MembershipPlan : AggregateRoot
    {
        public Guid Id { get; private set; }

        public Name Name { get; private set; }

        public Money Price { get; private set; }

        public int DiscountPercent { get; private set; }

        public bool FreeShipping { get; private set; }

        public MembershipDuration DurationInDays { get; private set; }

        public bool IsActive { get; private set; }

        private MembershipPlan()
        {
        }

        public MembershipPlan(
            Name name,
            Money price,
            int discountPercent,
            bool freeShipping,
            MembershipDuration durationInDays)
        {
            ArgumentNullException.ThrowIfNull(name);
            ArgumentNullException.ThrowIfNull(price);

            if (discountPercent is < 0 or > 100)
                throw new EShopDomainException(
                    "Discount percent must be between 0 and 100.");

            if (durationInDays.Days <= 0)
                throw new EShopDomainException(
                    "Membership duration must be greater than zero.");

            Id = Guid.NewGuid();
            Name = name;
            Price = price;
            DiscountPercent = discountPercent;
            FreeShipping = freeShipping;
            DurationInDays = durationInDays;
            IsActive = true;
        }

        public void ChangePrice(Money price)
        {
            ArgumentNullException.ThrowIfNull(price);

            Price = price;
        }

        public void ChangeDiscount(int percent)
        {
            if (percent is < 0 or > 100)
                throw new EShopDomainException();

            DiscountPercent = percent;
        }

        public void Disable()
        {
            IsActive = false;
        }

        public void Enable()
        {
            IsActive = true;
        }
    }
}
