using EShop.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Exceptions
{
    public class EShopNullException : EShopException
    {
        public object? Par { get; set; }
        public EShopNullException(string message = $"{nameof(Par)} is null.") : base(message)
        {
            
        }
    }
}
