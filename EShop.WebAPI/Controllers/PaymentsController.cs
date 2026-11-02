using _02.Application.CommentAgg.Commands.AddComment;
using _02.Application.PaymentAgg.Commands.InitialPayment;
using _02.Application.PaymentAgg.Commands.ProcessPayment;
using _02.Application.PaymentAgg.Commands.RefundPayment;
using EShop.Facade.Payment;
using EShop.Query.PaymentAgg.DTOs;
using EShop.Query.PaymentAgg.DTOs.Admin;
using EShop.Query.PaymentAgg.DTOs.Customer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EShop.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentFacade _paymentFacade;

        public PaymentsController(IPaymentFacade paymentFacade)
        {
            _paymentFacade = paymentFacade;
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<InitiatePaymentResult>> Create([FromBody] InitiatePaymentCommand command)
        {
            var result = await _paymentFacade.Initial(command);

            return StatusCode(StatusCodes.Status201Created, result);
        }

        [HttpPut("{paymentId:guid}/process")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PaymentCallbackResult>> Process(
            Guid paymentId,
            [FromBody] ProcessPaymentRequestDto request)
        {
            var command = new ProcessPaymentCallbackCommand(paymentId, request.Authority,request.Status);
            var result = await _paymentFacade.ProcessPaymentCallback(command);

            return Ok(result);
        }

        [HttpPut("{paymentId:guid}/refund")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PaymentCallbackResult>> Refund(
            Guid paymentId,
            [FromBody] RefundPaymentRequestDto request)
        {
            var command = new RefundPaymentCommand(paymentId, request.reason);
            var result = await _paymentFacade.Refund(command);

            return Ok(result);
        }

        [HttpGet("adminFilter")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(AdminPaymentFilterResult), StatusCodes.Status200OK)]
        public async Task<ActionResult<AdminPaymentFilterResult>> GetAllForAdmin(
            [FromQuery] AdminPaymentFilterParams filterParams)
        {
            var result = await _paymentFacade.FilterPaymentsForAdmin(filterParams);
            return Ok(result);
        }

        [HttpGet("{customerId:guid}/customerFilter")]
        [Authorize]
        [ProducesResponseType(typeof(CustomerPaymentFilterResult), StatusCodes.Status200OK)]
        public async Task<ActionResult<CustomerPaymentFilterResult>> GetAllForCustomer(Guid customerId,
            [FromQuery] CustomerPaymentFilterParams filterParams)
        {
            var result = await _paymentFacade.GetPaymentsForCustomer(customerId,filterParams);
            return Ok(result);
        }
        [HttpGet("{paymentId:guid}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(PaymentDetailDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<PaymentDetailDto?>> GetById(Guid paymentId)
        {
            var result = await _paymentFacade.GetById(paymentId);
            return Ok(result);
        }

        [HttpGet("{orderId:guid}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(OrderPaymentStatusDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<OrderPaymentStatusDto?>> GetStatusOfOrder(Guid orderId)
        {
            var result = await _paymentFacade.GetPaymentStatusOfOrder(orderId);
            return Ok(result);
        }


    }

    public record ProcessPaymentRequestDto(string Authority, string Status);
    public record RefundPaymentRequestDto(string reason);
}
