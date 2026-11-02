using _02.Application.CommentAgg.Commands.AddComment;
using _02.Application.CommentAgg.Commands.RejectComment;
using _02.Application.MembershipAgg.Commands.CancellMembership;
using _02.Application.MembershipAgg.Commands.ExpireMembership;
using _02.Application.MembershipAgg.Commands.PurchaseMembership;
using EShop.Facade.Membership;
using EShop.Query.CommentAgg.DTOs.Admin;
using EShop.Query.CustomerAgg.DTOs;
using EShop.Query.MembershipAgg.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EShop.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipController : ControllerBase
    {
        private readonly IMembershipFacade _membershipFacade;

        public MembershipController(IMembershipFacade membershipFacade)
        {
            _membershipFacade = membershipFacade;
        }

        [HttpPut("{membershipId:guid}/cancel")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Cancel(Guid membershipId)
        {
            var result = await _membershipFacade.Cancell(new CancellMembershipCommand(membershipId));
            return Ok(result);
        }

        [HttpPut("expire")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Expire()
        {
            var result = await _membershipFacade.Expire(new ExpireMembershipCommand());
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Purchase([FromBody] PurchaseMembershipCommand command)
        {
            var result = await _membershipFacade.Purchase(command);

            return StatusCode(StatusCodes.Status201Created, result);
        }

        [HttpGet("{customerId:guid}/active")]
        [Authorize]
        [ProducesResponseType(typeof(ActiveMembershipDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ActiveMembershipDto?>> GetActive(Guid customerId)
        {
            var result = await _membershipFacade.GetActiveMembership(customerId);
            if (result is null)
                return NotFound("Does not found active membership.");

            return Ok(result);
        }
        [HttpGet("{customerId:guid}/history")]
        [Authorize]
        [ProducesResponseType(typeof(CustomerMembershipHistoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CustomerMembershipHistoryDto>> GetHistory(Guid customerId)
        {
            var result = await _membershipFacade.GetMembershipHistory(customerId);

            return Ok(result);
        }
        [HttpGet("{id:guid}")]
        [Authorize(Roles="Admin")]
        [ProducesResponseType(typeof(MembershipAdminDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MembershipAdminDto?>> GetById(Guid id)
        {
            var result = await _membershipFacade.GetById(id);
            if (result is null)
                return NotFound($"Does not found membership with this id {id}.");

            return Ok(result);
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(MembershipFilterResult), StatusCodes.Status200OK)]
        public async Task<ActionResult<MembershipFilterResult>> GetAllForAdmin(
            [FromQuery] MembershipFilterParam filterParams)
        {
            var result = await _membershipFacade.GetForAdmin(filterParams);
            return Ok(result);
        }
    }
}
