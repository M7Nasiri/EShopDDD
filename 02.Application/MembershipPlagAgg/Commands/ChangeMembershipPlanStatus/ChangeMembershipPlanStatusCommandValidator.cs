using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.MembershipPlagAgg.Commands.ChangeMembershipPlanStatus
{
    public class ChangeMembershipPlanStatusCommandValidator : AbstractValidator<ChangeMembershipPlanStatusCommand>
    {
        public ChangeMembershipPlanStatusCommandValidator()
        {
            RuleFor(x => x.PlanId).NotEmpty();
        }
    }
}
