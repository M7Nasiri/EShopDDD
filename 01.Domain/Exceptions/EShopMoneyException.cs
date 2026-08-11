using EShop.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Exceptions
{
    public class EShopMoneyException : EShopException
    {
        public EShopMoneyException() : base("Money can not be negative")
        {
            
        }
    }
}
