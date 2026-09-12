using _01.Domain.Consts;
using EShop.Shared.Query.Filter;

namespace EShop.Query.PaymentAgg.DTOs.Admin;

public class AdminPaymentFilterParams : BaseFilterParam
{
    public string? Search { get; set; }
    public PaymentStatus? Status { get; set; }
}
