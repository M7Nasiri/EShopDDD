using _02.Application.CommentAgg.Commands.ApproveComment;
using _02.Application.ShipmentAgg.Commands.DeliverShip;
using EShop.Facade.Shipment;
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
    }
}
