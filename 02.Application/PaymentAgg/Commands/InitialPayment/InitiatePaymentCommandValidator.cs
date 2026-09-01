using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.PaymentAgg.Commands.InitialPayment
{
    public class InitiatePaymentCommandValidator : AbstractValidator<InitiatePaymentCommand>
    {
        public InitiatePaymentCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty().WithMessage("شناسه سفارش الزامی است.");

        }
    }
}
