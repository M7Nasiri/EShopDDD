using _01.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.ValueObjects
{
    public record MembershipDuration
    {
        public int Days { get; }

        public MembershipDuration(int days)
        {
            if (days <= 0)
                throw new EShopDomainException(
                    "Membership duration must be greater than zero.");

            Days = days;
        }

        public DateTime CalculateEndDate(DateTime startDate)
        {
            return startDate.AddDays(Days);
        }
    }
}
