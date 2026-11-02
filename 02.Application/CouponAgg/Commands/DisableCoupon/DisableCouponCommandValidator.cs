using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using _02.Application.CouponAgg.Commands.EnableCoupon;

namespace _02.Application.CouponAgg.Commands.DisableCoupon
{
    public class EnableCouponCommandValidator : AbstractValidator<EnableCouponCommand>
    {
        public EnableCouponCommandValidator()
        {
            RuleFor(x => x.CouponId)
                .NotEmpty().WithMessage("شناسه محصول الزامی است.");
        }
    }
}
