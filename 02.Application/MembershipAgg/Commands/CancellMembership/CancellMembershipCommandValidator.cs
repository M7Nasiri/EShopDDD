using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.MembershipAgg.Commands.CancellMembership
{
    public class CancellMembershipCommandValidator : AbstractValidator<CancellMembershipCommand>
    {
        public CancellMembershipCommandValidator()
        {
            RuleFor(x => x.MembershipId)
              .NotEmpty()
              .WithMessage("شناسه محصول الزامی است.");
        }
    }
}
