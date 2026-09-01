using _01.Domain.Entities.Aggregates.CommentAgg;
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

namespace _02.Application.CommentAgg.Commands.AddComment
{
    public class AddCommentCommandHandler : IBaseCommandHandler<AddCommentCommand>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;
        private readonly IProductRepository _productRepository;

        public AddCommentCommandHandler(
            ICommentRepository commentRepository,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser,
            IProductRepository productRepository)
        {
            _commentRepository = commentRepository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _productRepository = productRepository;
        }


        public async Task<OperationResult> Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {
            if (!_currentUser.IsValid())
            {
                throw new UnauthorizedAccessException(
                    "User must be authenticated.");
            }

            var customerId = new Guid(_currentUser.UserId.Value);
            var productId = new Guid(request.ProductId);

            var isExist =await _productRepository.ExistsAsync(request.ProductId,cancellationToken);
            if (!isExist)
                throw new EShopNullException("Product does not exist.");

            var alreadyCommented = await _commentRepository.ExistsByProductAndCustomerAsync(
               productId,
               customerId,
               cancellationToken);

            if (alreadyCommented)
            {
                throw new EShopDomainException(
                    "You have already submitted a comment for this product.");
            }


            var comment = new ProductComment(request.Text, productId, customerId);

            await _commentRepository.AddAsync(
            comment,
            cancellationToken);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return OperationResult.Success();

        }
    }
}
