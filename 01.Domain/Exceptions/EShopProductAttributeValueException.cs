using EShop.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Exceptions
{
    internal class EShopProductAttributeValueException : EShopException
    {
        public EShopProductAttributeValueException() : base("Value of prodcut attribute can not be empty.")
        {

        }
    }
    
}
