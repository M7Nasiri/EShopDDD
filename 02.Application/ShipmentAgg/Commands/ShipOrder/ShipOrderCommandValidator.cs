using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.ShipmentAgg.Commands.ShipOrder
{
    public sealed class ShipOrderCommandValidator : AbstractValidator<ShipOrderCommand>
    {
        public ShipOrderCommandValidator()
        {
            RuleFor(x => x.ShipmentId)
                .NotEmpty().WithMessage("شناسه مرسوله الزامی است.");

            RuleFor(x => x.TrackingCode)
                .NotEmpty().WithMessage("کد رهگیری پستی الزامی است.")
                .MaximumLength(100).WithMessage("طول کد رهگیری نمی‌تواند بیشتر از ۱۰۰ کاراکتر باشد.");
        }
    }
}
