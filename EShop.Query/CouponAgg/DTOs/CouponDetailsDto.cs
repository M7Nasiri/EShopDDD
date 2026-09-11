using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.CouponAgg.DTOs
{
    public class CouponDetailsDto
    {
        public string Code { get; set; } = string.Empty;
        public int Percent { get; set; }
        public Guid CreatedByUserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public int? UsageLimit { get; set; }
        public int UsedCount { get; set; }


        public bool IsExpired => DateTime.UtcNow >= EndDate;
        public bool IsExhausted => UsageLimit.HasValue && UsedCount >= UsageLimit.Value;
    }
}
