using FluentValidation;

namespace _02.Application.CommentAgg.Commands.RejectComment
{
    public class RejectCommentCommandValidator : AbstractValidator<RejectCommentCommand>
    {
        public RejectCommentCommandValidator()
        {
            RuleFor(x => x.CommentId)
             .NotEmpty()
             .WithMessage("شناسه نظر الزامی است.");

        }
    }
}
