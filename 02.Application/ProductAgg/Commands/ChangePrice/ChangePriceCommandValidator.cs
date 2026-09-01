using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.ProductAgg.Commands.ChangePrice
{
    public class ChangePriceCommandValidator : AbstractValidator<ChangePriceCommand>
    {
        public ChangePriceCommandValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.NewPrice).GreaterThan(0).WithMessage("قیمت جدید باید بزرگتر از صفر باشد.");
        }
    }
}
