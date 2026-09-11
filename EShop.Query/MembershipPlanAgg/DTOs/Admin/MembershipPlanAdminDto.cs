using System;
using System.Collections.Generic;
using System.Text;
using EShop.Shared.Query;

namespace EShop.Query.MembershipPlanAgg.DTOs.Admin
{
    public class MembershipPlanAdminDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int DiscountPercent { get; set; }
        public bool FreeShipping { get; set; }
        public int DurationInDays { get; set; }
        public bool IsActive { get; set; }
    }
}
