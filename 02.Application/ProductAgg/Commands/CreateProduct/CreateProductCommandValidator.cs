using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.ProductAgg.Commands.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("نام کالا الزامی است.")
                .MaximumLength(200).WithMessage("نام کالا نمی‌تواند بیش از ۲۰۰ کاراکتر باشد.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("توضیحات کالا الزامی است.")
                .MaximumLength(2000).WithMessage("توضیحات نمی‌تواند بیش از ۲۰۰۰ کاراکتر باشد.");

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("موجودی اولیه نمی‌تواند منفی باشد.");

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0).WithMessage("مبلغ کالا باید بیشتر از صفر باشد.");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("دسته‌بندی کالا الزامی است.");
        }
    }
}
