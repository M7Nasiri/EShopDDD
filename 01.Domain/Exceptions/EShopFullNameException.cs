using EShop.Shared.Domain.Exceptions;

namespace _01.Domain.Exceptions
{
    public class EShopFullNameException : EShopException
    {
        public EShopFullNameException() : base("Full Name Cannot be empty")
        {

        }
    }
}
