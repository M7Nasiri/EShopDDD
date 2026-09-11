using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.MembershipPlanAgg.DTOs
{
    public class MembershipPlanDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int DiscountPercent { get; set; }
        public bool FreeShipping { get; set; }
        public int DurationInDays { get; set; }
    }
}
