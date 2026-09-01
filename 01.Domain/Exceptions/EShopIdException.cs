using EShop.Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Exceptions
{
    public class EShopIdException : EShopException
    {
        public EShopIdException() : base("Id Cannot be empty.")
        {
            
        }
    }
}
