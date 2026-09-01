using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.CouponAgg.Commands.EnableCoupon
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
