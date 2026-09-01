using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.CategoryAgg.Commands.AddCategory
{
    public class AddCategoryCommandValidator : AbstractValidator<AddCategoryCommand>
    {
        public AddCategoryCommandValidator()
        {

            RuleFor(x => x.Name)
                .MinimumLength(5).WithMessage("Min 5 Chars.")
                .MaximumLength(100).WithMessage("Max 100 Chars.");
        }
    }
}
