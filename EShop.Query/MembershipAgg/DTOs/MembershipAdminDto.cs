using EShop.Shared.Query;

namespace EShop.Query.MembershipAgg.DTOs;

public class MembershipAdminDto : BaseDto
{
    public Guid CustomerId { get; set; }
    public string CustomerFullName { get; set; } = string.Empty;
    public string CustomerPhoneNumber { get; set; } = string.Empty;
    public Guid PlanId { get; set; }
    public string PlanTitle { get; set; } = string.Empty;
    public int DiscountPercent { get; set; }
    public bool FreeShipping { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; } = string.Empty;
}