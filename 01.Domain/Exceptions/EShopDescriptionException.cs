using EShop.Shared.Domain.Exceptions;

namespace _01.Domain.Exceptions
{
    public class EShopDescriptionException : EShopException
    {
        public EShopDescriptionException() : base("Description cannot be empty.")
        {

        }
    }
}
