using EShop.Query.OrderAgg.DTOs;
using EShop.Shared.Query.Filter;

namespace EShop.Query.PaymentAgg.DTOs.Admin;

public class AdminPaymentFilterResult:
    BaseFilter<AdminPaymentListItemDto, AdminPaymentFilterParams>;