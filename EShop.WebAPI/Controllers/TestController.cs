using EShop.Query.CartAgg.GetCartByCustomerId;
using EShop.Query.CommentAgg.DTOs.Admin;
using EShop.Query.CommentAgg.DTOs.Customer;
using EShop.Query.CommentAgg.GetAllCommentForAdmin;
using EShop.Query.CommentAgg.GetApprovedCommentForProduct;
using EShop.Query.CommentAgg.GetCommentForCustomer;
using EShop.Query.CommentAgg.GetWhoApprovedComment;
using EShop.Query.OrderAgg.DTOs;
using EShop.Query.OrderAgg.GetCustomersOrdersForAdmin;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EShop.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private IMediator _mediator;

        public TestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //[HttpGet("{customerId:guid}")]
        //public async Task<IActionResult> GetCartByCustomerId([FromRoute]Guid customerId)
        //{
        //    var result = await _mediator.Send(new GetCartByCustomerIdQuery(customerId));

        //    if (result == null)
        //    {
        //        return NotFound(new { message = "سبد خریدی برای این مشتری یافت نشد." });
        //    }
        //    return Ok(result);
        //}

        //[HttpGet("{customerId:guid?}")]
        //public async Task<ActionResult<CustomersOrdersForAdminFilterResult>> GetAllOrders(
        //    [FromRoute] Guid customerId,
        //    [FromQuery] CustomersOrdersForAdminFilterParams filterParams,
        //    CancellationToken cancellationToken)
        //{
        //    filterParams.CustomerId = customerId;
        //    var result = await _mediator.Send(new GetCustomersOrdersForAdminQuery(filterParams),cancellationToken);
        //    if (result == null)
        //    {
        //        return NotFound(new { message = "سفارشی یافت نشد." });
        //    }
        //    return Ok(result);

        //}
        //[HttpGet("{productId:guid}")]
        //public async Task<IActionResult> GetCartByCustomerId([FromRoute] Guid productId)
        //{
        //    var result = 
        //        await _mediator.Send(new GetApprovedCommentForProductQuery(productId));

        //    if (result == null)
        //    {
        //        return NotFound(new { message = "سبد خریدی برای این مشتری یافت نشد." });
        //    }
        //    return Ok(result);
        //}

        //[HttpGet("{customerId:guid?}")]
        //public async Task<ActionResult<CommentCustomerFilterResult>> GetAllOrders(
        //    [FromRoute] Guid customerId,
        //    [FromQuery] CommentCustomerFilterParams filterParams,
        //    CancellationToken cancellationToken)
        //{

        //    var result = await _mediator.Send(new GetCommentForCustomerQuery(customerId,filterParams), cancellationToken);
        //    if (result == null)
        //    {
        //        return NotFound(new { message = "سفارشی یافت نشد." });
        //    }
        //    return Ok(result);
        //}

        //[HttpGet]
        //public async Task<ActionResult<CommentFilterResult>> GetAllOrders(
        //    [FromQuery] CommentFilterParams filterParams,
        //    CancellationToken cancellationToken)
        //{

        //    var result = await _mediator.Send(new GetAllCommentForAdminQuery(filterParams), cancellationToken);
        //    if (result == null)
        //    {
        //        return NotFound(new { message = "سفارشی یافت نشد." });
        //    }
        //    return Ok(result);
        //}
        [HttpGet("{commentId:guid?}")]
        public async Task<ActionResult<WhoApprovedCommentDto>> GetAllOrders(
            [FromRoute] Guid commentId,
            CancellationToken cancellationToken)
        {

            var result = await _mediator.Send(new GetWhoApprovedCommentQuery(commentId), cancellationToken);
            if (result == null)
            {
                return NotFound(new { message = "سفارشی یافت نشد." });
            }
            return Ok(result);
        }
    }
}
