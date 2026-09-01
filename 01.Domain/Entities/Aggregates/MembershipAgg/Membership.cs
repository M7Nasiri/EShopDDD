using _01.Domain.Consts;
using _01.Domain.Entities.Aggregates.MembershipPlanAgg;
using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Domain;

namespace _01.Domain.Entities.Aggregates.MembershipAgg
{
    public sealed class Membership : AggregateRoot
    {
        public Guid Id { get; private set; }

        public Guid CustomerId { get; private set; }

        public Guid PlanId { get; private set; }
        public Name PlanTitle { get; private set; }


        public int DiscountPercent { get; private set; }

        public bool FreeShipping { get; private set; }

        public DomainDate StartDate { get; private set; }

        public DomainDate EndDate { get; private set; }

        public MembershipStatus Status { get; private set; }

        private Membership()
        {
        }

        public Membership(
            Guid customerId,
            MembershipPlan plan,
            DomainDate startDate)
        {
            ArgumentNullException.ThrowIfNull(customerId);
            ArgumentNullException.ThrowIfNull(plan);
            ArgumentNullException.ThrowIfNull(startDate);

            if (!plan.IsActive)
                throw new EShopDomainException(
                    "Membership plan is inactive.");

            Id = Guid.New();
            CustomerId = customerId;
            PlanId = plan.Id;
            DiscountPercent = plan.DiscountPercent;
            FreeShipping = plan.FreeShipping;
            StartDate = startDate;
            PlanTitle = plan.Name;

            EndDate =
                new DomainDate(
                    startDate.Value.AddDays(
                        plan.DurationInDays.Days));

            Status = MembershipStatus.Active;
        }

        public bool IsActiveAt(DomainDate date)
        {
            ArgumentNullException.ThrowIfNull(date);

            return Status == MembershipStatus.Active &&
                   date >= StartDate &&
                   date < EndDate;
        }

        public bool IsActiveNow() =>
            IsActiveAt(DomainDate.Now);

        public DiscountSnapshot CreateDiscountSnapshot()
        {
            if (!IsActiveNow())
                throw new EShopDomainException(
                    "Membership is not active.");

            if (DiscountPercent <= 0)
                throw new EShopDomainException(
                    "Membership has no discount.");

            return new DiscountSnapshot(
                PlanTitle.Value,
                DiscountPercent,
                FreeShipping)
                ;
        }

        public void Cancel()
        {
            if (Status == MembershipStatus.Cancelled)
                return;

            if (Status == MembershipStatus.Expired)
                throw new EShopDomainException(
                    "Expired membership cannot be cancelled.");

            Status = MembershipStatus.Cancelled;
        }

        public void Expire()
        {
            if (Status != MembershipStatus.Active)
                return;

            Status = MembershipStatus.Expired;
        }
    }
}
