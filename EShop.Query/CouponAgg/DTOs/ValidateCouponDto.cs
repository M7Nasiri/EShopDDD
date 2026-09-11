using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Query.CouponAgg.DTOs
{
    public class ValidateCouponDto
    {
        public Guid CouponId { get; set; }
        public string Code { get; set; } = string.Empty;
        public int Percent { get; set; }
        public bool IsValid { get; set; }
        public string Message { get; set; } = string.Empty;

        public static ValidateCouponDto Valid(Guid id, string code, int percent) => new()
        {
            CouponId = id,
            Code = code,
            Percent = percent,
            IsValid = true,
            Message = $"کد تخفیف {code}  با موفقیت تایید شد."
        };

        public static ValidateCouponDto Invalid(string message) => new()
        {
            IsValid = false,
            Message = message
        };
    }
}
