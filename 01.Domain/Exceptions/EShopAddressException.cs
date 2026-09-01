using EShop.Shared.Domain.Exceptions;

namespace _01.Domain.Exceptions
{
    public class EShopAddressException : EShopException
    {
        public EShopAddressException(string message = "Address Cannot be empty.") : base(message)
        {

        }
    }
}
