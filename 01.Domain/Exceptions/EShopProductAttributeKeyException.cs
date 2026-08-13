using EShop.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Exceptions
{
    public class EShopProductAttributeKeyException : EShopException
    {
        public EShopProductAttributeKeyException():base("Key of prodcut attribute can not be empty.")
        {
            
        }
    }
}
