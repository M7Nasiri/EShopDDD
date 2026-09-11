using EShop.Shared.Query.Filter;

namespace EShop.Query.MembershipAgg.DTOs;

public class MembershipFilterParam : BaseFilterParam
{
    public Guid? CustomerId { get; set; }
    public string? CustomerSearch { get; set; }
    public string? Status { get; set; }
    public bool? JustActive { get; set; }
}