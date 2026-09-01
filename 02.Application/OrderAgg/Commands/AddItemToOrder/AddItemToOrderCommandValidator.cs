using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.OrderAgg.Commands.AddItemToOrder
{
    public class AddItemToOrderCommandValidator : AbstractValidator<AddItemToOrderCommand>
    {
        public AddItemToOrderCommandValidator()
        {
            RuleFor(x => x.ProductId)
               .NotEmpty()
               .WithMessage("شناسه محصول الزامی است.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("تعداد محصول باید بیشتر از صفر باشد.");
        }
    }
}
