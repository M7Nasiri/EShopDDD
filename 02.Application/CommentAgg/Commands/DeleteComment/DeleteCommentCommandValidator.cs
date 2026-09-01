using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.CommentAgg.Commands.DeleteComment
{
    public class DeleteCommentCommandValidator : AbstractValidator<DeleteCommentCommand> 
    {
        public DeleteCommentCommandValidator()
        {
            RuleFor(x => x.CommentId)
            .NotEmpty()
            .WithMessage("شناسه نظر الزامی است.");
        }
    }
}
