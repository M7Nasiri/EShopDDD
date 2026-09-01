using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.CustomerAgg.Commands.RegisterCustomer
{
    public class RegisterCustomerCommandValidator : AbstractValidator<RegisterCustomerCommand>
    {
        public RegisterCustomerCommandValidator()
        {
            RuleFor(x => x.UserName)
            .MinimumLength(5).WithMessage("Min 5 Chars.")
            .MaximumLength(100).WithMessage("Max 100 Chars.");

            RuleFor(x => x.Name)
            .MinimumLength(5).WithMessage("Min 5 Chars.")
            .MaximumLength(100).WithMessage("Max 100 Chars.");

            RuleFor(x => x.Family)
            .MinimumLength(5).WithMessage("Min 5 Chars.")
            .MaximumLength(100).WithMessage("Max 100 Chars.");


            //Need To ComeBsck
            RuleFor(x => x.Email)
            .MinimumLength(5).WithMessage("Min 5 Chars.")
            .MaximumLength(100).WithMessage("Max 100 Chars.");

            RuleFor(x => x.Password)
            .MinimumLength(5).WithMessage("Min 5 Chars.")
            .MaximumLength(100).WithMessage("Max 100 Chars.");

        }
    }
}
