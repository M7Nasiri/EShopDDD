using EShop.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Exceptions
{
    public class EShopMoneyPriceException : EShopException
    {
        public EShopMoneyPriceException(string message = "Price can not be null.") : base(message)
        {
            
        }
    }
}
