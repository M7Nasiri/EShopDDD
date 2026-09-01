using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.CouponAgg.Commands.DeleteCoupon
{
    public class DeleteCouponCommandValidator : AbstractValidator<DeleteCouponCommand>
    {
        public DeleteCouponCommandValidator()
        {
            RuleFor(x => x.CouponId)
                .NotEmpty().WithMessage("شناسه محصول الزامی است.");
        }
    }
}
