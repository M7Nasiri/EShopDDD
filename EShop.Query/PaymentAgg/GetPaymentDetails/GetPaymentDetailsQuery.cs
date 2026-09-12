using EShop.Query.PaymentAgg.DTOs;
using EShop.Shared.Application;
using System;
using System.Collections.Generic;
using System.Text;
using EShop.Shared.Query;

namespace EShop.Query.PaymentAgg.GetPaymentDetails
{
    public record GetPaymentDetailsQuery(Guid PaymentId) : IQuery<PaymentDetailDto?>;
}
