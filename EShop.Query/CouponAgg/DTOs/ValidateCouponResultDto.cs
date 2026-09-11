namespace EShop.Query.CouponAgg.DTOs;

public record ValidateCouponResultDto(
    Guid Id,
    string Code,
    int Percent,
    bool IsValid,
    string? Message = null
);