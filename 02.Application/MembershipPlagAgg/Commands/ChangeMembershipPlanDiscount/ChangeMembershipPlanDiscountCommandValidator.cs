using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.MembershipPlagAgg.Commands.ChangeMembershipPlanDiscount
{
    public class ChangeMembershipPlanPriceCommandValidator : AbstractValidator<ChangeMembershipPlanPriceCommand>
    {
        public ChangeMembershipPlanPriceCommandValidator()
        {
            RuleFor(x => x.PlanId).NotEmpty();
            RuleFor(x => x.NewDiscountPercent).InclusiveBetween(0, 100).WithMessage("درصد تخفیف باید بین ۰ تا ۱۰۰ باشد.");
        }
    }
}