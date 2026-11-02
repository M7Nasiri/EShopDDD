using _02.Application.CommentAgg.Commands.ApproveComment;
using _02.Application.ShipmentAgg.Commands.DeliverShip;
using _02.Application.ShipmentAgg.Commands.ReturnShipment;
using _02.Application.ShipmentAgg.Commands.ShipOrder;
using EShop.Facade.Shipment;
using EShop.Query.CommentAgg.DTOs.Admin;
using EShop.Query.ShipmentAgg.DTOs;
using EShop.Query.ShipmentAgg.DTOs.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EShop.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentsController : ControllerBase
    {
        private readonly IShipmentFacade _shipmentFacade;

        public ShipmentsController(IShipmentFacade shipmentFacade)
        {
            _shipmentFacade = shipmentFacade;
        }

        [HttpPut("{shipmentId:guid}/deliver")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Deliver(Guid shipmentId)
        {
            var result = await _shipmentFacade.Deliver(new DeliverShipmentCommand(shipmentId));
            return Ok(result);
        }

        [HttpPut("{shipmentId:guid}/return")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Return(Guid shipmentId)
        {
            var result = await _shipmentFacade.Return(new ReturnShipmentCommand(shipmentId));
            return Ok(result);
        }

        [HttpPut("{shipmentId:guid}/ship")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Ship(Guid shipmentId
        , [FromBody] ShipRequestDto dto)
        {
            var result = await _shipmentFacade.Ship(new ShipOrderCommand(shipmentId,dto.TrackingCode));
            return Ok(result);
        }
        [HttpGet("order/{orderId:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(OrderShipmentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderShipmentDto?>> GetByOrderId(Guid orderId)
        {
            var result = await _shipmentFacade.GetByOrderId(orderId);
            if (result is null)
                return NotFound("Does not found any shipment!");

            return Ok(result);
        }
        [HttpGet("{shipmentId:guid}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(AdminShipmentDetailsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AdminShipmentDetailsDto?>> GetById(Guid shipmentId)
        {
            var result = await _shipmentFacade.GetById(shipmentId);
            if (result is null)
                return NotFound("Does not found any shipment!");

            return Ok(result);
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ShipmentAdminFilterResult), StatusCodes.Status200OK)]
        public async Task<ActionResult<ShipmentAdminFilterResult>> GetAllForAdmin(
            [FromQuery] ShipmentAdminFilterParams filterParams)
        {
            var result = await _shipmentFacade.GetAllForAdmin(filterParams);
            return Ok(result);
        }

        [HttpGet("{trackingCode}/track")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(TrackingShipmentResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TrackingShipmentResultDto?>> TrackByTrackingCode(string trackingCode)
        {
            var result = await _shipmentFacade.TrackByTrackingCode(trackingCode);
            if (result is null)
                return NotFound("Does not found any shipment!");

            return Ok(result);
        }
    }

    public record ShipRequestDto(string TrackingCode);
}
