using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.ShipmentAgg.Commands.ReturnShipment
{
    public sealed class ReturnShipmentCommandValidator : AbstractValidator<ReturnShipmentCommand>
    {
        public ReturnShipmentCommandValidator()
        {
            RuleFor(x => x.ShipmentId)
                .NotEmpty().WithMessage("شناسه مرسوله الزامی است.");
        }
    }
}
