using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.CartAgg.Commands.RemoveItem
{
    public class RemoveItemCommandValidator : AbstractValidator<RemoveItemCommand>
    {
        public RemoveItemCommandValidator()
        {
            RuleFor(x => x.ProductId)
           .NotEmpty()
           .WithMessage("شناسه محصول الزامی است.");
        }
    }
}
