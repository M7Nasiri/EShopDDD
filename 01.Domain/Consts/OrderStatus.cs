using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Consts
{
    public enum OrderStatus
    {
        Draft = 0,
        PendingPayment = 1,
        Paid = 2,
        Shipped = 3,
        Delivered = 4,
        Cancelled = 5
    }
}
