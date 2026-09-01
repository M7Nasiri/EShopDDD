using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.CustomerAgg.Commands.AddCustomerAddress
{
    public class AddCustomerAddressCommandValidator : AbstractValidator<AddCustomerAddressCommand>
    {
        public AddCustomerAddressCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("عنوان آدرس الزامی است.").MaximumLength(100);
            RuleFor(x => x.Province).NotEmpty().WithMessage("استان الزامی است.");
            RuleFor(x => x.City).NotEmpty().WithMessage("شهر الزامی است.");
            RuleFor(x => x.Street).NotEmpty().WithMessage("خیابان الزامی است.");
            RuleFor(x => x.Plaque).NotEmpty().WithMessage("پلاک الزامی است.");
            RuleFor(x => x.PostalCode)
                .NotEmpty().WithMessage("کد پستی الزامی است.")
                .Length(10).WithMessage("کد پستی باید دقیقاً ۱۰ رقم باشد.")
                .Matches(@"^\d{10}$").WithMessage("کد پستی باید فقط شامل اعداد باشد.");
        }
    }
}
