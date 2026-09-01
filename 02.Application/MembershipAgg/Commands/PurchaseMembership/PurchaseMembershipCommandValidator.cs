using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.MembershipAgg.Commands.PurchaseMembership
{
    public class PurchaseMembershipCommandValidator : AbstractValidator<PurchaseMembershipCommand>
    {
        public PurchaseMembershipCommandValidator()
        {
            RuleFor(m=>m.PlanId).NotEmpty();
        }
    }
}
