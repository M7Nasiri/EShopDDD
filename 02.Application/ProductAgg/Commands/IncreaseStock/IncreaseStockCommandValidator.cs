using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.ProductAgg.Commands.IncreaseStock
{
    public class IncreaseStockCommandValidator : AbstractValidator<IncreaseStockCommand>
    {
        public IncreaseStockCommandValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("تعداد افزایشی باید حداقل ۱ باشد.");
        }
    }
}
