using EShop.Shared.Domain.Exceptions;

namespace _01.Domain.Exceptions
{
    public class EShopProductAttributeKeyException : EShopException
    {
        public EShopProductAttributeKeyException() : base("Key of prodcut attribute can not be empty.")
        {

        }
    }
}
