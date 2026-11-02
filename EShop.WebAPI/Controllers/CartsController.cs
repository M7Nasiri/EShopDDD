using _01.Domain.Entities.Aggregates.CartAgg;
using _02.Application.CartAgg.Commands.AddItem;
using _02.Application.CartAgg.Commands.Clear;
using _02.Application.CartAgg.Commands.MergeGuestCart;
using _02.Application.CartAgg.Commands.RemoveItem;
using EShop.Facade.Cart;
using EShop.Query.CartAgg.DTOs;
using EShop.Query.CommentAgg.DTOs.Admin;
using EShop.Shared.Application;
using EShop.Shared.Application.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EShop.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartFacade _cartFacade;
        public CartsController(ICartFacade cartFacade)
        {
            _cartFacade = cartFacade;
        }

        [HttpPost("items")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCart([FromBody]AddItemCommand command)
        {
            var result = await _cartFacade.AddItem(command);
            return Ok(result);
        }

        [HttpDelete("items/{productId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveItem(Guid productId)
        {
            var result = await _cartFacade.RemoveItem(
                new RemoveItemCommand(productId));

            return Ok(result);
        }


        [HttpDelete("items")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ClearCart()
        {
            var command = new ClearCartCommand();
            var result = await _cartFacade.Clear(command);
            return Ok(result);
        }

        [HttpPost("merge")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> MegeGuestCart()
        {
            var command = new MergeGuestCartCommand();
            var result = await _cartFacade.MergeGuestCart(command);
            return Ok(result);
        }


        [HttpGet("current")]
        [Authorize]
        [ProducesResponseType(typeof(CartDto),StatusCodes.Status200OK)]
        public async Task<ActionResult<CartDto?>> GetCurrent()
        {
            var result = await _cartFacade.GetCurrent();
            if (result == null)
                return Ok(new CartDto { Items = new() });

            return Ok(result);
        }



        [HttpGet("admin")]
        [Authorize(Roles="Admin")]
        [ProducesResponseType(typeof(CartFilterResult), StatusCodes.Status200OK)]
        public async Task<ActionResult<CartFilterResult>> GetByFilter([FromQuery]CartFilterParam filterParam)
        {
            var result = await _cartFacade.GetByFilter(filterParam);
            return Ok(result);
        }

    }
}
