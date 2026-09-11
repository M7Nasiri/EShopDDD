using _01.Domain.Exceptions;
using EShop.Shared.Domain;

namespace _01.Domain.ValueObjects
{
    public sealed record Money : ValueObject
    {
        public decimal Amount { get; }

        public Money(decimal amount)
        {
            if (amount < 0)
                throw new EShopMoneyException();

            Amount = decimal.Round(
                amount,
                2,
                MidpointRounding.AwayFromZero);
        }

        public static Money Zero => new(0);

        public static Money operator +(Money left, Money right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return new Money(left.Amount + right.Amount);
        }

        public static Money operator -(Money left, Money right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            if (left.Amount < right.Amount)
                throw new EShopMoneyException();

            return new Money(left.Amount - right.Amount);
        }

        public static Money operator *(Money price, int count)
        {
            ArgumentNullException.ThrowIfNull(price);

            if (count < 0)
                throw new EShopDomainException(
                    "Count cannot be negative.");

            return new Money(price.Amount * count);
        }

        public Money ApplyPercentageDiscount(int percent)
        {
            if (percent is < 0 or > 100)
                throw new EShopDomainException("درصد باید عددی بین 1 تا 100 باشد .");

            var discount =
                decimal.Round(
                    Amount * percent / 100m,
                    2,
                    MidpointRounding.AwayFromZero);

            return new Money(Amount - discount);
        }

        public static explicit operator decimal(Money money)
        {
            ArgumentNullException.ThrowIfNull(money);
            return money?.Amount??0m;
        }

        public static explicit operator Money(decimal amount) =>
            new(amount);
    }


}
