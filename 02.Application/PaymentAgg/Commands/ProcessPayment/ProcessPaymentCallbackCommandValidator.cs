using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.PaymentAgg.Commands.ProcessPayment
{
    public class ProcessPaymentCallbackCommandValidator : AbstractValidator<ProcessPaymentCallbackCommand>
    {
        public ProcessPaymentCallbackCommandValidator()
        {
            RuleFor(x => x.PaymentId)
                .NotEmpty().WithMessage("شناسه پرداخت الزامی است.");

            RuleFor(x => x.Authority)
                .NotEmpty().WithMessage("کد رهگیری درگاه الزامی است.");
        }
    }
}
