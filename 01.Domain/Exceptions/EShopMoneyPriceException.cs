using EShop.Shared.Domain.Exceptions;

namespace _01.Domain.Exceptions
{
    public class EShopMoneyPriceException : EShopException
    {
        public EShopMoneyPriceException(string message = "Price can not be null.") : base(message)
        {

        }
    }
}
