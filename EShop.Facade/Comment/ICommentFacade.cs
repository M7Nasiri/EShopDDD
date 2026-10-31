using _02.Application.CommentAgg.Commands.AddComment;
using _02.Application.CommentAgg.Commands.ApproveComment;
using _02.Application.CommentAgg.Commands.DeleteComment;
using _02.Application.CommentAgg.Commands.EditComment;
using _02.Application.CommentAgg.Commands.RejectComment;
using EShop.Query.CommentAgg.DTOs.Admin;
using EShop.Query.CommentAgg.DTOs.Customer;
using EShop.Query.CommentAgg.DTOs.Product;
using EShop.Shared.Application;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EShop.Query.CommentAgg.GetAllCommentForAdmin;
using EShop.Query.CommentAgg.GetApprovedCommentForProduct;
using EShop.Query.CommentAgg.GetCommentForCustomer;
using EShop.Query.CommentAgg.GetWhoApprovedComment;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EShop.Facade.Comment
{
    public interface ICommentFacade
    {
        //Command
        Task<OperationResult> Add(AddCommentCommand command);
        Task<OperationResult> Approve(ApproveCommentCommand command);
        Task<OperationResult> Delete(DeleteCommentCommand command);
        Task<OperationResult> Edit(EditCommentCommand command);
        Task<OperationResult> Reject(RejectCommentCommand command);


        //Query
        Task<CommentFilterResult> GetAllForAdmin(CommentFilterParams filterParams);
        Task<IReadOnlyList<ProductCommentSummaryDto?>> GetApprovedForProduct(Guid productId);
        Task<CommentCustomerFilterResult> GetForCustomer(Guid customerId, CommentCustomerFilterParams filterParams);
        Task<WhoApprovedCommentDto?> GetWhoApproved(Guid commentId);

    }

    public class CommentFacade : ICommentFacade
    {
        private readonly IMediator _mediator;

        public CommentFacade(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<OperationResult> Add(AddCommentCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<OperationResult> Approve(ApproveCommentCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<OperationResult> Delete(DeleteCommentCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<OperationResult> Edit(EditCommentCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<OperationResult> Reject(RejectCommentCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<CommentFilterResult> GetAllForAdmin(CommentFilterParams filterParams)
        {
            return await _mediator.Send(new GetAllCommentForAdminQuery(filterParams));
        }

        public async Task<IReadOnlyList<ProductCommentSummaryDto?>> GetApprovedForProduct(Guid productId)
        {
            return await _mediator.Send(new GetApprovedCommentForProductQuery(productId));
        }

        public async Task<CommentCustomerFilterResult> GetForCustomer(Guid customerId, CommentCustomerFilterParams filterParams)
        {
            return await _mediator.Send(new GetCommentForCustomerQuery(customerId,filterParams));
        }

        public async Task<WhoApprovedCommentDto?> GetWhoApproved(Guid commentId)
        {
            return await _mediator.Send(new GetWhoApprovedCommentQuery(commentId));
        }
    }
}
