using EShop.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Exceptions
{
    public class EShopDescriptionException : EShopException
    {
        public EShopDescriptionException() : base("Description cannot be empty.")
        {
            
        }
    }
}
