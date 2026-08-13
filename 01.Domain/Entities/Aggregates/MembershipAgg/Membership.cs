using _01.Domain.Consts;
using _01.Domain.Entities.Aggregates.MembershipPlanAgg;
using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Abstractions.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Entities.Aggregates.MembershipAgg
{
    public sealed class Membership : AggregateRoot
    {
        public Id Id { get; private set; }

        public Id CustomerId { get; private set; }

        public Id PlanId { get; private set; }

        public int DiscountPercent { get; private set; }

        public bool FreeShipping { get; private set; }

        public DomainDate StartDate { get; private set; }

        public DomainDate EndDate { get; private set; }

        public MembershipStatus Status { get; private set; }

        private Membership()
        {
        }

        public Membership(
            Id id,
            Id customerId,
            MembershipPlan plan,
            DomainDate startDate)
        {
            ArgumentNullException.ThrowIfNull(id);
            ArgumentNullException.ThrowIfNull(customerId);
            ArgumentNullException.ThrowIfNull(plan);
            ArgumentNullException.ThrowIfNull(startDate);

            if (!plan.IsActive)
                throw new EShopDomainException(
                    "Membership plan is inactive.");

            Id = id;
            CustomerId = customerId;
            PlanId = plan.Id;
            DiscountPercent = plan.DiscountPercent;
            FreeShipping = plan.FreeShipping;
            StartDate = startDate;

            EndDate =
                new DomainDate(
                    startDate.Value.AddDays(
                        plan.DurationInDays));

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
                "Membership",
                DiscountPercent);
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
