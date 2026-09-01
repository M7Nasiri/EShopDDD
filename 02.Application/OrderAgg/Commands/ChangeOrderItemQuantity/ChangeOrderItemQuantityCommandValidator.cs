using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.OrderAgg.Commands.ChangeOrderItemQuantity
{
    internal class ChangeOrderItemQuantityCommandValidator : AbstractValidator<ChangeOrderItemQuantityCommand>
    {
        public ChangeOrderItemQuantityCommandValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty().WithMessage("شناسه محصول الزامی است.");
            RuleFor(x => x.NewQuantity).GreaterThan(0).WithMessage("تعداد باید حداقل ۱ باشد.");
        }
    }
}
