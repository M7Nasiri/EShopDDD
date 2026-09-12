using EShop.Query.PaymentAgg.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using EShop.Shared.Query;

namespace EShop.Query.PaymentAgg.GetPaymentOrderStatus
{
    public record GetOrderPaymentStatusQuery(Guid OrderId) : IQuery<OrderPaymentStatusDto?>;
}
