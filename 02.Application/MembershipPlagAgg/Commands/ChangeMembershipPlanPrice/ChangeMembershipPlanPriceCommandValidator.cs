using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.MembershipPlagAgg.Commands.ChangeMembershipPlanPrice
{
    public class ChangeMembershipPlanPriceCommandValidator : AbstractValidator<ChangeMembershipPlanPriceCommand>
    {
        public ChangeMembershipPlanPriceCommandValidator()
        {
            RuleFor(x => x.PlanId).NotEmpty();
            RuleFor(x => x.NewPrice).GreaterThanOrEqualTo(0).WithMessage("قیمت نمی تواند منفی باشد .");
        }
    }
}
