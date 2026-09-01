using FluentValidation;

namespace _02.Application.CommentAgg.Commands.ApproveComment
{
    public class RejectCommentCommandValidator : AbstractValidator<ApproveCommentCommand>
    {
        public RejectCommentCommandValidator()
        {
            RuleFor(x => x.CommentId)
             .NotEmpty()
             .WithMessage("شناسه نظر الزامی است.");

        }
    }
}
