using EShop.Query.OrderAgg.DTOs;
using EShop.Shared.Query;
using System;
using System.Collections.Generic;
using System.Text;
using EShop.Shared.Query.Filter;
using EShop.Query.PaymentAgg.DTOs.Admin;

namespace EShop.Query.PaymentAgg.GetAdminPaymentsFilter
{
    public class GetAdminPaymentsFilterQuery(AdminPaymentFilterParams filterParams) :
        QueryFilter<AdminPaymentFilterResult, AdminPaymentFilterParams>(filterParams);
}
