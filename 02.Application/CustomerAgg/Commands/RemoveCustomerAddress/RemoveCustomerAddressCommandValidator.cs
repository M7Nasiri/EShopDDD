using FluentValidation;

namespace _02.Application.CustomerAgg.Commands.RemoveCustomerAddress
{
    public class RemoveCustomerAddressCommandValidator : AbstractValidator<RemoveCustomerAddressCommand>
    {
        public RemoveCustomerAddressCommandValidator()
        {
            RuleFor(x => x.PostalCode).NotEmpty().WithMessage("عنوان آدرس الزامی است.")
                .MaximumLength(10)
                .MinimumLength(10);
        }
    }
}
