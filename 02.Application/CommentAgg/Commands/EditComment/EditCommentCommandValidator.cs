using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.CommentAgg.Commands.EditComment
{
    public class EditCommentCommandValidator : AbstractValidator<EditCommentCommand>
    {
        public EditCommentCommandValidator()
        {
            RuleFor(x => x.CommentId)
             .NotEmpty()
             .WithMessage("شناسه نظر الزامی است.");

            RuleFor(x => x.Text)
            .MinimumLength(5).WithMessage("Min 5 Chars.")
            .MaximumLength(100).WithMessage("Max 100 Chars.");
        }
    }
}
