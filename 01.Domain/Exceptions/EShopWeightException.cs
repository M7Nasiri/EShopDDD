using EShop.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Exceptions
{
    public class EShopWeightException : EShopException
    {
        public EShopWeightException() : base("Weight cannot be negative.")
        {
            
        }
    }
}
