using EShop.Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Exceptions
{
    public class EShopLoginException : EShopException
    {
        public EShopLoginException():base("لاگین کنید")
        {
            
        }
    }
}
