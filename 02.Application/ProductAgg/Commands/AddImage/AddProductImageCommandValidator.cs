using FluentValidation;

namespace _02.Application.ProductAgg.Commands.AddImage
{
    public class AddProductImageCommandValidator : AbstractValidator<AddProductImageCommand>
    {
        public AddProductImageCommandValidator()
        {
            RuleFor(b => b.ImageFile)
                .NotNull().WithMessage("Image don't select.");

            RuleFor(b => b.Sequence)
                .GreaterThanOrEqualTo(0);
        }
    }
}