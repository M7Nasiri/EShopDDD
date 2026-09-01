using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.CouponAgg.Commands.AddCoupon
{
    public class AddCouponCommandValidator : AbstractValidator<AddCouponCommand>
    {
        public AddCouponCommandValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("شناسه محصول الزامی است.")
                .MinimumLength(4).WithMessage("حداقل طول کد تخفیف ، باید 4 باشد");
                

            RuleFor(x => x.Percent)
                .GreaterThan(0).WithMessage("درصد تخفیف ، باید بزرگتر از صفر باشد .")
                .LessThan(100).WithMessage("درصد تخفیف ، باید کوچکتر از صد باشد.");

            RuleFor(x => x.UsageLimit)
                .GreaterThan(0).WithMessage("تعداد کوپن ، باید بزرگتر از صفر باشد .");

        }
    }
}
