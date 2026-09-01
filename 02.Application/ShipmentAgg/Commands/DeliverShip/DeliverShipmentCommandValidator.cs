using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.ShipmentAgg.Commands.DeliverShip
{
    public sealed class DeliverShipmentCommandValidator : AbstractValidator<DeliverShipmentCommand>
    {
        public DeliverShipmentCommandValidator()
        {
            RuleFor(x => x.ShipmentId)
                .NotEmpty().WithMessage("شناسه مرسوله الزامی است.");
        }
    }
}
