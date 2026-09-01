using EShop.Shared.Domain.Exceptions;

namespace _01.Domain.Exceptions
{
    public class EShopDomainException : EShopException
    {
        public EShopDomainException()
        {

        }
        public EShopDomainException(string message) : base(message)
        {

        }
    }
}
