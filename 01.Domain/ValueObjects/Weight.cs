using _01.Domain.Exceptions;

namespace _01.Domain.ValueObjects
{
    public record Weight
    {
        public static Weight Zero => new(0);

        public double Kg { get; }

        public Weight(double kg)
        {
            if (kg < 0)
                throw new EShopWeightException();
            Kg = kg;
        }

        public Weight Multiply(int count) => new(Kg * count);

        public static Weight operator +(Weight a, Weight b) => new(a.Kg + b.Kg);

        public static bool operator >(Weight a, Weight b) => a.Kg > b.Kg;
        public static bool operator <(Weight a, Weight b) => a.Kg < b.Kg;

        public override string ToString() => $"{Kg:N2} kg";
    }

}
