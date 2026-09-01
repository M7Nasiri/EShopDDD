using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.MembershipAgg.Commands.ExpireMembership
{
    public class ExpireMembershipCommandValidator : AbstractValidator<ExpireMembershipCommand>
    {
        public ExpireMembershipCommandValidator()
        {
            
        }
    }
}
