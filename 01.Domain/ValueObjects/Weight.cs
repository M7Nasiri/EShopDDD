using _01.Domain.Exceptions;
using EShop.Shared.Domain;

namespace _01.Domain.ValueObjects
{
    public sealed record Weight : ValueObject
    {
        public static Weight Zero => new(0);

        public double Kg { get; }

        public Weight(double kg)
        {
            if (kg < 0)
                throw new EShopWeightException();
            Kg = Math.Round(kg, 3);
        }

        public Weight Multiply(int count) => new(Kg * count);

        public static Weight operator +(Weight a, Weight b) => new(a.Kg + b.Kg);

        public static bool operator >(Weight a, Weight b) => a.Kg > b.Kg;
        public static bool operator <(Weight a, Weight b) => a.Kg < b.Kg;
        public static bool operator >=(Weight a, Weight b) => a.Kg >= b.Kg;
        public static bool operator <=(Weight a, Weight b) => a.Kg <= b.Kg;
        public override string ToString() => $"{Kg:N2} kg";
    }

}
