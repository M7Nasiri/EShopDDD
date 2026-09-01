using EShop.Shared.Domain.Exceptions;

namespace _01.Domain.Exceptions
{
    internal class EShopProductAttributeValueException : EShopException
    {
        public EShopProductAttributeValueException() : base("Value of prodcut attribute can not be empty.")
        {

        }
    }

}
