using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Exceptions
{
    public class EShopCouponException : EShopAddressException
    {
        public EShopCouponException() : base("Coupon can not be less than 0 or greater than 100")
        {
            
        }
    }
}
