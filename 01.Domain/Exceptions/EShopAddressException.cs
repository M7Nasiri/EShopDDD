using EShop.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Exceptions
{
    public class EShopAddressException : EShopException
    {
        public EShopAddressException(string message= "Address Cannot be empty.") : base(message)
        {
            
        }
    }
}
