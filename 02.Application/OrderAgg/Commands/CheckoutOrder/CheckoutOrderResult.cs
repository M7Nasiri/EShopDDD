using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.OrderAgg.Commands.CheckoutOrder
{
    public sealed record CheckoutOrderResult(Guid OrderId, decimal TotalAmount);
}
