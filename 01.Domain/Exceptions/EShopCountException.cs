using EShop.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Exceptions
{
    public class EShopCountException:EShopException
    {
        public EShopCountException() : base("Count can not be less than 1")
        {
            
        }
    }
}
