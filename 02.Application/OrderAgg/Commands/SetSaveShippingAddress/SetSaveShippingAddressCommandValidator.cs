using FluentValidation;

namespace _02.Application.OrderAgg.Commands.SetSaveShippingAddress
{
    internal class SetSaveShippingAddressCommandValidator : AbstractValidator<SetSaveShippingAddressCommand>
    {
        public SetSaveShippingAddressCommandValidator()
        {
            RuleFor(x => x.PostalCode).NotEmpty().Length(10).WithMessage("کد پستی باید ۱۰ رقم باشد.");
            RuleFor(x => x.Cost).GreaterThanOrEqualTo(0);

        }
    }
}
