using EShop.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Exceptions
{
    public class EShopDomainException : EShopException
    {
        public EShopDomainException()
        {
            
        }
        public EShopDomainException(string message):base(message)
        {
            
        }
    }
}
