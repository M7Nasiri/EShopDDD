using _02.Application.CommentAgg.Commands.AddComment;
using _02.Application.CommentAgg.Commands.ApproveComment;
using _02.Application.CommentAgg.Commands.DeleteComment;
using _02.Application.CommentAgg.Commands.EditComment;
using _02.Application.CommentAgg.Commands.RejectComment;
using EShop.Facade.Comment;
using EShop.Query.CommentAgg.DTOs.Admin;
using EShop.Query.CommentAgg.DTOs.Customer;
using EShop.Query.CommentAgg.DTOs.Product;
using EShop.Query.CustomerAgg.DTOs;
using EShop.Shared.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EShop.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {

        [ApiController]
        [Route("api/[controller]")]
        public class CommentsController : ControllerBase
        {
            private readonly ICommentFacade _commentFacade;

            public CommentsController(ICommentFacade commentFacade)
            {
                _commentFacade = commentFacade;
            }

            #region Public & Customer Endpoints

            /// <summary>
            /// دریافت نظرات تایید شده یک محصول خاص (مناسب صفحه جزئیات محصول)
            /// URL: GET /api/comments/product/{productId}
            /// </summary>
            [HttpGet("product/{productId:guid}")]
            [ProducesResponseType(typeof(IReadOnlyList<ProductCommentSummaryDto>), StatusCodes.Status200OK)]
            public async Task<ActionResult<IReadOnlyList<ProductCommentSummaryDto?>>> GetApprovedByProduct(
                Guid productId)
            {
                var result = await _commentFacade.GetApprovedForProduct(productId);
                return Ok(result);
            }

            /// <summary>
            /// دریافت نظرات ارسال‌شده توسط یک مشتری (پنل کاربری)
            /// URL: GET /api/comments/customer/{customerId}
            /// </summary>
            [HttpGet("customer/{customerId:guid}")]
            [Authorize]
            [ProducesResponseType(typeof(CommentCustomerFilterResult), StatusCodes.Status200OK)]
            public async Task<ActionResult<CommentCustomerFilterResult>> GetCustomerComments(
                Guid customerId,
                [FromQuery] CommentCustomerFilterParams filterParams)
            {
                var result = await _commentFacade.GetForCustomer(customerId, filterParams);
                return Ok(result);
            }

            /// <summary>
            /// ثبت دیدگاه جدید
            /// URL: POST /api/comments
            /// </summary>
            [HttpPost]
            [Authorize]
            [ProducesResponseType(StatusCodes.Status201Created)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            public async Task<IActionResult> Create([FromBody] AddCommentCommand command)
            {
                var result = await _commentFacade.Add(command);

                // در صورت موفقیت، کد 201 Created بازمی‌گردانیم
                return StatusCode(StatusCodes.Status201Created, result);
            }

            /// <summary>
            /// ویرایش متن دیدگاه توسط کاربر
            /// URL: PUT /api/comments/{commentId}
            /// </summary>
            [HttpPut("{commentId:guid}")]
            [Authorize]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            public async Task<IActionResult> Edit(
                Guid commentId,
                [FromBody] EditCommentRequestDto request)
            {
                // شناسه از Route خوانده شده و با بدنه ترکیب می‌شود
                var command = new EditCommentCommand(commentId, request.Text);
                var result = await _commentFacade.Edit(command);

                return Ok(result);
            }

            #endregion

            #region Admin / Moderation Endpoints

            /// <summary>
            /// دریافت تمام نظرات با فیلتر برای ادمین
            /// URL: GET /api/comments/admin
            /// </summary>
            [HttpGet("admin")]
            [Authorize(Roles = "Admin,Moderator")]
            [ProducesResponseType(typeof(CommentFilterResult), StatusCodes.Status200OK)]
            public async Task<ActionResult<CommentFilterResult>> GetAllForAdmin(
                [FromQuery] CommentFilterParams filterParams)
            {
                var result = await _commentFacade.GetAllForAdmin(filterParams);
                return Ok(result);
            }

            /// <summary>
            /// استعلام اینکه چه کسی این کامنت را تایید کرده است
            /// URL: GET /api/comments/{commentId}/moderator
            /// </summary>
            [HttpGet("{commentId:guid}/moderator")]
            [Authorize(Roles = "Admin")]
            [ProducesResponseType(typeof(WhoApprovedCommentDto), StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public async Task<ActionResult<WhoApprovedCommentDto>> GetWhoApproved(Guid commentId)
            {
                var result = await _commentFacade.GetWhoApproved(commentId);
                if (result is null)
                    return NotFound("اطلاعات تاییدکننده یافت نشد.");

                return Ok(result);
            }

            /// <summary>
            /// تایید یک دیدگاه توسط ادمین
            /// URL: PUT /api/comments/{commentId}/approve
            /// </summary>
            [HttpPut("{commentId:guid}/approve")]
            [Authorize(Roles = "Admin,Moderator")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            public async Task<IActionResult> Approve(Guid commentId)
            {
                var result = await _commentFacade.Approve(new ApproveCommentCommand(commentId));
                return Ok(result);
            }

            /// <summary>
            /// رد یک دیدگاه توسط ادمین
            /// URL: PUT /api/comments/{commentId}/reject
            /// </summary>
            [HttpPut("{commentId:guid}/reject")]
            [Authorize(Roles = "Admin,Moderator")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            public async Task<IActionResult> Reject(Guid commentId)
            {
                var result = await _commentFacade.Reject(new RejectCommentCommand(commentId));
                return Ok(result);
            }

            /// <summary>
            /// حذف یک دیدگاه
            /// URL: DELETE /api/comments/{commentId}
            /// </summary>
            [HttpDelete("{commentId:guid}")]
            [Authorize(Roles = "Admin,Moderator")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            public async Task<IActionResult> Delete(Guid commentId)
            {
                var result = await _commentFacade.Delete(new DeleteCommentCommand(commentId));
                return Ok(result);
            }

            #endregion
        }

        /// <summary>
        /// مدل کمکی (DTO) برای ویرایش که شناسه داخل آن نیست و از URL گرفته می‌شود
        /// </summary>
        public record EditCommentRequestDto(string Text);
    }
}
