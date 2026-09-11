using _01.Domain.Exceptions;
using EShop.Shared.Domain;

namespace _01.Domain.ValueObjects
{
    public record MembershipDuration : ValueObject
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
