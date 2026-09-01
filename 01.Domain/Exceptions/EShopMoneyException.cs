using EShop.Shared.Domain.Exceptions;

namespace _01.Domain.Exceptions
{
    public class EShopMoneyException : EShopException
    {
        public EShopMoneyException() : base("Money can not be negative")
        {

        }
    }
}
