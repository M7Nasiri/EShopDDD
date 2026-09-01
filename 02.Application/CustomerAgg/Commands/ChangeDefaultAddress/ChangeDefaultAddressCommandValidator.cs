using FluentValidation;

namespace _02.Application.CustomerAgg.Commands.ChangeDefaultAddress
{
    internal class ChangeDefaultAddressCommandValidator : AbstractValidator<ChangeDefaultAddressCommand>
    {
        public ChangeDefaultAddressCommandValidator()
        {
            RuleFor(x => x.PostalCode).NotEmpty().WithMessage("عنوان آدرس الزامی است.")
                .MaximumLength(10)
                .MinimumLength(10);
        }
    }
}
