using EShop.Shared.Domain.Exceptions;

namespace _01.Domain.Exceptions
{
    public class EShopCountException : EShopException
    {
        public EShopCountException() : base("Count can not be less than zero")
        {

        }
    }
}
