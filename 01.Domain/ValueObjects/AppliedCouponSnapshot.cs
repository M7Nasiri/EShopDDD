using _01.Domain.Exceptions;
using EShop.Shared.Domain;
namespace _01.Domain.ValueObjects
{
    public sealed record AppliedCouponSnapshot : ValueObject
    {
        public Guid CouponId { get; }
        public string Code { get; }
        public int Percent { get; }

        public AppliedCouponSnapshot(
            string code,
            int percent)
        {
            CouponId = Guid.NewGuid();

            if (string.IsNullOrWhiteSpace(code))
                throw new EShopCouponException();

            if (percent is < 1 or > 100)
                throw new EShopCouponException();

            Code = code.Trim();
            Percent = percent;
        }
    }
}
