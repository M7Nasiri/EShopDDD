using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Consts
{
    public enum PaymentStatus
    {
        Pending = 0,
        Processing = 1,
        Succeeded = 2,
        Failed = 3,
        Refunded = 4
    }
}
