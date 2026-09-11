using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Shared.Application.Exceptions
{
    public class EShopIdentityException(string message) : Exception(message);
}
