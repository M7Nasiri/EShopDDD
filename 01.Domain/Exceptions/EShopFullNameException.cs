using EShop.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Exceptions
{
    public class EShopFullNameException : EShopException
    {
        public EShopFullNameException() : base("Full Name Cannot be empty")
        {
            
        }
    }
}
