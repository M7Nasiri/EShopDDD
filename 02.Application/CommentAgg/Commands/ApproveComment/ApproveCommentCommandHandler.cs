using _01.Domain.Entities.Aggregates.CommentAgg.Repository;
using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Application;
using EShop.Shared.Application.Interfaces.Authentication;
using EShop.Shared.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.CommentAgg.Commands.ApproveComment
{
    public class ApproveCommentCommandHandler : IBaseCommandHandler<ApproveCommentCommand>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;


        public ApproveCommentCommandHandler(
            ICommentRepository commentRepository,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser)
        {
            _commentRepository = commentRepository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }
        public async Task<OperationResult> Handle(ApproveCommentCommand request, CancellationToken cancellationToken)
        {
            if (!_currentUser.IsValid())
            {
                throw new UnauthorizedAccessException(
                    "User must be authenticated.");
            }

            if (!_currentUser.CanModerateComments)
            {
                throw new UnauthorizedAccessException(
                    "You do not have permission to approve comments.");
            }



            var userId = new Guid(_currentUser.UserId.Value);
            var comment = await _commentRepository.GetTracking(request.CommentId, cancellationToken);

            if (comment is null)
                throw new EShopNullException("Comment was no found");

            comment.Approve(userId);

            await _unitOfWork.SaveChangesAsync(
               cancellationToken);

            return OperationResult.Success();

        }
    }
}
