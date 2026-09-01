using EShop.Shared.Domain.Exceptions;

namespace _01.Domain.Exceptions
{
    public class EShopWeightException : EShopException
    {
        public EShopWeightException() : base("Weight cannot be negative.")
        {

        }
    }
}
