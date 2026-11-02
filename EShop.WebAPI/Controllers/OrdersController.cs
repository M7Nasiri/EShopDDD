using _02.Application.CommentAgg.Commands.AddComment;
using _02.Application.CommentAgg.Commands.EditComment;
using _02.Application.OrderAgg.Commands.AddItemToOrder;
using _02.Application.OrderAgg.Commands.ApplyCouponToOrder;
using _02.Application.OrderAgg.Commands.ApplyMembershipDiscount;
using _02.Application.OrderAgg.Commands.ChangeOrderItemQuantity;
using _02.Application.OrderAgg.Commands.CheckoutOrder;
using _02.Application.OrderAgg.Commands.OrderLogistics;
using _02.Application.OrderAgg.Commands.ProcessOrderPaymentCallback;
using _02.Application.OrderAgg.Commands.RemoveCouponFromOrder;
using _02.Application.OrderAgg.Commands.RemoveItemFromOrder;
using _02.Application.OrderAgg.Commands.RemoveMembershipDiscount;
using _02.Application.OrderAgg.Commands.SetSaveShippingAddress;
using EShop.Facade.Order;
using EShop.Query.CommentAgg.DTOs.Customer;
using EShop.Query.OrderAgg.DTOs;
using EShop.Query.OrderAgg.DTOs.GetActiveDraftOrder;
using EShop.Query.OrderAgg.DTOs.GetAdminOrderDetails;
using EShop.Query.OrderAgg.DTOs.GetCustomerOrderDetails;
using EShop.Shared.Application.Interfaces.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EShop.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderFacade _orderFacade;
        private readonly ICurrentUser currentUser;

        public OrdersController(IOrderFacade orderFacade, ICurrentUser currentUser)
        {
            _orderFacade = orderFacade;
            this.currentUser = currentUser;
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] AddItemToOrderCommand command)
        {
            var result = await _orderFacade.AddItem(command);

            return StatusCode(StatusCodes.Status201Created, result);
        }

        [HttpPut("{couponCode}/coupon")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ApplyCoupon(
            string couponCode)
        {

            var command = new ApplyCouponToOrderCommand(couponCode);
            var result = await _orderFacade.ApplyCoupon(command);

            return Ok(result);
        }
        [HttpPut("membershipDiscount")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ApplyMembershipDiscount()
        {

            var command = new ApplyMembershipDiscountCommand();
            var result = await _orderFacade.ApplyMembershipDiscount(command);

            return Ok(result);
        }

        [HttpPut("{productId}/changeQuantity")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangeQuantity(Guid productId,
            [FromBody] ChangeOrderItemQuantityDto dto)
        {

            var command = new ChangeOrderItemQuantityCommand(productId,dto.NewQuantity);
            var result = await _orderFacade.ChangeQuantityOfItem(command);

            return Ok(result);
        }

        [HttpPut("checkout")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CheckoutOrderResult>> Checkout()
        {

            var command = new CheckoutOrderCommand();
            var result = await _orderFacade.Checkout(command);

            return Ok(result);
        }

        [HttpPut("ship")]
        [Authorize(Roles="Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Ship()
        {

            var command = new ShipOrderCommand();
            var result = await _orderFacade.Logistics(command);

            return Ok(result);
        }
        [HttpPut("{orderId:guid}/paid")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Paid(Guid orderId)
        {

            var command = new ProcessOrderPaymentCallbackCommand(orderId);
            var result = await _orderFacade.ProcessPaymentCallback(command);

            return Ok(result);
        }

        [HttpPut("removeCoupon")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveCoupon()
        {

            var command = new RemoveCouponFromOrderCommand();
            var result = await _orderFacade.RemoveCoupon(command);

            return Ok(result);
        }

        [HttpPut("removeItem/{productId:guid}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveItem(Guid productId)
        {

            var command = new RemoveItemFromOrderCommand(productId);
            var result = await _orderFacade.RemoveItem(command);

            return Ok(result);
        }

        [HttpPut("removeMembershipDiscount")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveMembershipDiscount()
        {

            var command = new RemoveMembershipDiscountCommand();
            var result = await _orderFacade.RemoveMembershipDiscount(command);

            return Ok(result);
        }
        [HttpPut("saveShippingAddress/{postalCode}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SaveShippingAddress(string postalCode,
        [FromBody] SaveShippingAddressDto dto)
        {

            var command = new SetSaveShippingAddressCommand(postalCode,dto.Cost);
            var result = await _orderFacade.SetSaveShippingAddress(command);

            return Ok(result);
        }

        [HttpGet("{customerId:guid}/draft")]
        [Authorize]
        [ProducesResponseType(typeof(DraftOrderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DraftOrderDto?>> GetActiveDraft(Guid customerId)
        {
            var result = await _orderFacade.GetActiveDraft(customerId);
            if (result is null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("admin/{orderId:guid}")]
        [Authorize(Roles="Admin")]
        [ProducesResponseType(typeof(AdminOrderDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AdminOrderDetailDto?>> GetByIdForAdmin(Guid orderId)
        {
            var result = await _orderFacade.GetDetailOfOrderForAdmin(orderId);
            if (result is null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("customer/{orderId:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(CustomerOrderDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CustomerOrderDetailDto?>> GetById(Guid orderId)
        {
            var result = await _orderFacade.GetDetailOfOrderForCustomer(orderId,
                currentUser.UserId.Value);
            if (result is null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("customerFilter")]
        [Authorize]
        [ProducesResponseType(typeof(CustomerOrdersFilterResult), StatusCodes.Status200OK)]
        public async Task<ActionResult<CustomerOrdersFilterResult>> GetAllForCustomer(
            [FromQuery] CustomerOrdersFilterParams filterParams)
        {
            var result = await _orderFacade.GetCustomerOrderList(filterParams);
            return Ok(result);
        }

        [HttpGet("adminFilter")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(CustomersOrdersForAdminFilterResult), StatusCodes.Status200OK)]
        public async Task<ActionResult<CustomersOrdersForAdminFilterResult>> GetAllForAdmin(
            [FromQuery] CustomersOrdersForAdminFilterParams filterParams)
        {
            var result = await _orderFacade.GetCustomerOrderListForAdmin(filterParams);
            return Ok(result);
        }

    }

    public record ChangeOrderItemQuantityDto(int NewQuantity);
    public record SaveShippingAddressDto(decimal Cost);
}
