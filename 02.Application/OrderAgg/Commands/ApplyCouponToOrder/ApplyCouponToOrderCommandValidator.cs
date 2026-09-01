using FluentValidation;

namespace _02.Application.OrderAgg.Commands.ApplyCouponToOrder
{
    public class ApplyCouponToOrderCommandValidator : AbstractValidator<ApplyCouponToOrderCommand>
    {
        public ApplyCouponToOrderCommandValidator()
        {
            RuleFor(x => x.CouponCode).NotEmpty().WithMessage("کد کپن الزامی است.");

        }
    }
}
