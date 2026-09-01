using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.CommentAgg.Commands.AddComment
{
    public class AddCommentCommandValidator : AbstractValidator<AddCommentCommand>
    {
        public AddCommentCommandValidator()
        {
            RuleFor(x => x.ProductId)
              .NotEmpty()
              .WithMessage("شناسه محصول الزامی است.");


            RuleFor(x => x.Text)
               .MinimumLength(5).WithMessage("Min 5 Chars.")
               .MaximumLength(100).WithMessage("Max 100 Chars.");
        }
    }
}
