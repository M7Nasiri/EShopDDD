using EShop.Shared.Query.Filter;

namespace EShop.Query.CouponAgg.DTOs;

public class CouponFilterParams : BaseFilterParam
{
    public string? Search { get; set; }
    public bool? IsActive { get; set; }
    public bool? JustValid { get; set; } // کوپن‌هایی که منقضی یا تمام نشده‌اند
}