using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using _02.Application.MembershipPlagAgg.Commands.ChangeMembershipPlanPrice;

namespace _02.Application.MembershipPlagAgg.Commands.ChangeMembershipPlanDiscount
{
    public class ChangeMembershipPlanDiscountCommandValidator : AbstractValidator<ChangeMembershipPlanDiscountCommand>
    {
        public ChangeMembershipPlanDiscountCommandValidator()
        {
            RuleFor(x => x.PlanId).NotEmpty();
            RuleFor(x => x.NewDiscountPercent).InclusiveBetween(0, 100).WithMessage("درصد تخفیف باید بین ۰ تا ۱۰۰ باشد.");
        }
    }
}