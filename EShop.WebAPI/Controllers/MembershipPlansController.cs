using _02.Application.CommentAgg.Commands.AddComment;
using _02.Application.CommentAgg.Commands.EditComment;
using _02.Application.MembershipPlagAgg.Commands.ChangeMembershipPlanDiscount;
using _02.Application.MembershipPlagAgg.Commands.ChangeMembershipPlanPrice;
using _02.Application.MembershipPlagAgg.Commands.ChangeMembershipPlanStatus;
using _02.Application.MembershipPlagAgg.Commands.CreateMembershipPlan;
using EShop.Facade.MembershipPlan;
using EShop.Query.CommentAgg.DTOs.Admin;
using EShop.Query.CustomerAgg.DTOs;
using EShop.Query.MembershipPlanAgg.DTOs;
using EShop.Query.MembershipPlanAgg.DTOs.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EShop.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipPlansController : ControllerBase
    {
        private readonly IMembershipPlanFacade _membershipPlanFacade;

        public MembershipPlansController(IMembershipPlanFacade membershipPlanFacade)
        {
            _membershipPlanFacade = membershipPlanFacade;
        }

        [HttpPut("discount/{planId:guid}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePlanDiscount(
            Guid planId,
            [FromBody] EditPlanDiscountRequestDto request)
        {
            var command = new ChangeMembershipPlanDiscountCommand(planId, request.NewDiscountPercent);
            var result = await _membershipPlanFacade.ChangePlanDiscount(command);

            return Ok(result);
        }

        [HttpPut("price/{planId:guid}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePlanPrice(
            Guid planId,
            [FromBody] EditPlanPriceRequestDto request)
        {
            var command = new ChangeMembershipPlanPriceCommand(planId, request.NewPrice);
            var result = await _membershipPlanFacade.ChangePlanPrice(command);

            return Ok(result);
        }

        [HttpPut("status/{planId:guid}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePlanStatus(
            Guid planId,
            [FromBody] EditPlanStatusRequestDto request)
        {
            var command = new ChangeMembershipPlanStatusCommand(planId, request.Status);
            var result = await _membershipPlanFacade.ChangePlanStatus(command);

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles="Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateMembershipPlanCommand command)
        {
            var result = await _membershipPlanFacade.Add(command);
            return StatusCode(StatusCodes.Status201Created, result);
        }

        [HttpGet("active")]
        [ProducesResponseType(typeof(IReadOnlyList<MembershipPlanDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<MembershipPlanDto>>> GetActivePlan()
        {
            var result = await _membershipPlanFacade.GetActivePlan();
            return Ok(result);
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(CustomerFilterParams), StatusCodes.Status200OK)]
        public async Task<ActionResult<MembershipPlanFilterParam>> GetAllForAdmin(
            [FromQuery] MembershipPlanFilterParam filterParams)
        {
            var result = await _membershipPlanFacade.GetAllForAdmin(filterParams);

            return Ok(result);
        }

        [HttpGet("admin/{id:guid}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(MembershipPlanAdminDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MembershipPlanAdminDto>> GetById(Guid id)
        {
            var result = await _membershipPlanFacade.GetByIdForAdmin(id);
            if (result is null)
                return NotFound("پلن یافت نشد.");

            return Ok(result);
        }
        [HttpGet("lookup")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(IReadOnlyList<MembershipPlanLookupDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<MembershipPlanLookupDto>>> GetPlansLookup()
        {
            var result = await _membershipPlanFacade.GetPlansLookup();

            return Ok(result);
        }

    }

    public record EditPlanDiscountRequestDto(int NewDiscountPercent);
    public record EditPlanPriceRequestDto(decimal NewPrice);
    public record EditPlanStatusRequestDto(bool Status);
}
