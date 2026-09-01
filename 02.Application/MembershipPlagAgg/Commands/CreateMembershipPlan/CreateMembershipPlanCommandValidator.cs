using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.MembershipPlagAgg.Commands.CreateMembershipPlan
{
    public class CreateMembershipPlanCommandValidator : AbstractValidator<CreateMembershipPlanCommand>
    {
        public CreateMembershipPlanCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("نام پلان الزامی است.").MaximumLength(150);
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0).WithMessage("قیمت نمی‌تواند منفی باشد.");
            RuleFor(x => x.Percent).InclusiveBetween(0, 100).WithMessage("درصد تخفیف باید بین ۰ تا ۱۰۰ باشد.");
            RuleFor(x => x.DurationInDays).GreaterThan(0).WithMessage("مدت زمان اشتراک باید بزرگتر از صفر باشد.");
        }
    }
}
