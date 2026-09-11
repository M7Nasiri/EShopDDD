using EShop.Shared.Query.Filter;

namespace EShop.Query.MembershipPlanAgg.DTOs.Admin;

public class MembershipPlanFilterParam : BaseFilterParam
{
    public string? Search { get; set; }
    public bool? IsActive { get; set; }
}