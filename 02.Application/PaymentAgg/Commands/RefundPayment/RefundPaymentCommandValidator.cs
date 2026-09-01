using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.PaymentAgg.Commands.RefundPayment
{
    public class RefundPaymentCommandValidator : AbstractValidator<RefundPaymentCommand>
    {
        public RefundPaymentCommandValidator()
        {
            RuleFor(x => x.PaymentId)
                .NotEmpty().WithMessage("شناسه پرداخت الزامی است.");

            RuleFor(x => x.Reason)
                .NotEmpty().WithMessage("دلیل استرداد وجه الزامی است.")
                .MaximumLength(500);
        }
    }
}
