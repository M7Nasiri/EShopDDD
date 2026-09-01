using FluentValidation;

namespace _02.Application.OrderAgg.Commands.ProcessOrderPaymentCallback
{
    internal class ProcessOrderPaymentCallbackCommandValidator : AbstractValidator<ProcessOrderPaymentCallbackCommand>
    {
        public ProcessOrderPaymentCallbackCommandValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty().WithMessage("شناسه سفارش الزامی است.");
            RuleFor(x => x.TrackingNumber).NotEmpty().WithMessage("کد پیگیری الزامی است.");
        }
    }
}
