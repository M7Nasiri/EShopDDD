using _02.Application.CartAgg.Commands.AddItem;
using _02.Application.CartAgg.Commands.Clear;
using _02.Application.CategoryAgg.Commands.EditCategory;
using _02.Application.CouponAgg.Commands.AddCoupon;
using _02.Application.CouponAgg.Commands.DeleteCoupon;
using _02.Application.CouponAgg.Commands.DisableCoupon;
using _02.Application.CouponAgg.Commands.EnableCoupon;
using EShop.Facade.Coupon;
using EShop.Query.CartAgg.DTOs;
using EShop.Query.CouponAgg.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EShop.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponsController : ControllerBase
    {
        private readonly ICouponFacade _copounFacade;

        public CouponsController(ICouponFacade copounFacade)
        {
            _copounFacade = copounFacade;
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] AddCouponCommand command)
        {
            var result = await _copounFacade.Add(command);
            return Ok(result);
        }

        [HttpDelete("{couponId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Remove(Guid couponId)
        {
            var command = new DeleteCouponCommand(couponId);
            var result = await _copounFacade.Delete(command);
            return Ok(result);
        }

        [HttpPut("{couponId:guid}/disable")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Disable(Guid couponId)
        {
            var command = new DisableCouponCommand(couponId);
            var result = await _copounFacade.Disable(command);
            return Ok(result);
        }
        [HttpPut("{couponId:guid}/enable")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Enable(Guid couponId)
        {
            var command = new EnableCouponCommand(couponId);
            var result = await _copounFacade.Enable(command);
            return Ok(result);
        }

        [HttpGet("coupon/{code}")]
        [ProducesResponseType(typeof(CouponDetailsDto), StatusCodes.Status200OK)]
        [ProducesResponseType( StatusCodes.Status404NotFound)]

        public async Task<ActionResult<CouponDetailsDto?>> GetByCode(string code)
        {
            var result = await _copounFacade.GetByCode(code);
            if(result is null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(CouponFilterResult), StatusCodes.Status200OK)]
        public async Task<ActionResult<CouponFilterResult>> GetForAdmin([FromQuery] CouponFilterParams filterParams)
        {
            var result = await _copounFacade.GetCouponsForAdmin(filterParams);
            return Ok(result);
        }
        [HttpGet("validate/{code}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ValidateCouponDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<ValidateCouponDto>> Validate(string code)
        {
            var result = await _copounFacade.Validate(code);
            return Ok(result);
        }
    }
}
