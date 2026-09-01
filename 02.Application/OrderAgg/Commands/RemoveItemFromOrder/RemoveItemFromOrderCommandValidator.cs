using _02.Application.OrderAgg.Commands.ChangeOrderItemQuantity;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.OrderAgg.Commands.RemoveItemFromOrder
{
    internal class RemoveItemFromOrderCommandValidator : AbstractValidator<ChangeOrderItemQuantityCommand>
    {
        public RemoveItemFromOrderCommandValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty().WithMessage("شناسه محصول الزامی است.");
        }
    }
}
