using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.MembershipAgg.DTOs
{
    public class ActiveMembershipDto
    {
        public Guid MembershipId { get; set; }
        public Guid PlanId { get; set; }
        public string PlanTitle { get; set; } = string.Empty;
        public int DiscountPercent { get; set; }
        public bool FreeShipping { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int RemainingDays { get; set; }
    }
}
