using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.CouponAgg.DTOs
{
    public class CouponAdminDetailsDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public int Percent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public int? UsageLimit { get; set; }

    }
}
