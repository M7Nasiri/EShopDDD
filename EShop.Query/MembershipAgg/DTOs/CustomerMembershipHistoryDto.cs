namespace EShop.Query.MembershipAgg.DTOs;

public class CustomerMembershipHistoryDto
{
    public Guid Id { get; set; }
    public string PlanTitle { get; set; } = string.Empty;
    public int DiscountPercent { get; set; }
    public bool FreeShipping { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool IsCurrentlyActive { get; set; }
}