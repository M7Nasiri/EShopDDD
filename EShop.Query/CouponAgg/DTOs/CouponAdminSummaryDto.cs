using EShop.Shared.Query;

namespace EShop.Query.CouponAgg.DTOs;

public class CouponAdminSummaryDto : BaseDto
{
    public string Code { get; set; } = string.Empty;
    public int Percent { get; set; }
    public Guid CreatedByUserId { get; set; }
    public string CreatorFullName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
    public int? UsageLimit { get; set; }
    public int UsedCount { get; set; }

    // فیلد محاسباتی برای نمایش وضعیت در UI ادمین
    public bool IsExpired => DateTime.UtcNow >= EndDate;
    public bool IsExhausted => UsageLimit.HasValue && UsedCount >= UsageLimit.Value;
}