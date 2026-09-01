using _01.Domain.Entities.Aggregates.CommentAgg.Repository;
using _01.Domain.Entities.Aggregates.ProductAgg.Repository;
using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Application;
using EShop.Shared.Application.Interfaces.Authentication;
using EShop.Shared.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.CommentAgg.Commands.EditComment
{
    public class EditCommentCommandHandler : IBaseCommandHandler<EditCommentCommand>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;


        public EditCommentCommandHandler(
            ICommentRepository commentRepository,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser)
        {
            _commentRepository = commentRepository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }
        public async Task<OperationResult> Handle(EditCommentCommand request, CancellationToken cancellationToken)
        {
            if (!_currentUser.IsValid())
            {
                throw new UnauthorizedAccessException(
                    "User must be authenticated.");
            }

            var customerId = _currentUser.UserId.Value;
            var comment = await _commentRepository.GetTracking(request.CommentId, cancellationToken);

            if (comment is null)
                throw new EShopNullException("Comment was no found");

            if (!comment.IsOwnedBy(customerId))
            {
                throw new UnauthorizedAccessException(
                    "You cannot edit another customer's comment.");
            }

            comment.EditText(request.Text);

            await _unitOfWork.SaveChangesAsync(
               cancellationToken);

            return OperationResult.Success();
        }
    }
}
