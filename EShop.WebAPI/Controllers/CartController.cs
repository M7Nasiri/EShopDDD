using _02.Application.CartAgg.Commands.AddItem;
using _02.Application.CartAgg.Commands.Clear;
using _02.Application.CartAgg.Commands.MergeGuestCart;
using _02.Application.CartAgg.Commands.RemoveItem;
using EShop.Facade.Cart;
using EShop.Query.CartAgg.DTOs;
using EShop.Shared.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EShop.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartFacade _cartFacade;
        public CartController(ICartFacade cartFacade)
        {
            _cartFacade = cartFacade;
        }

        [HttpPost("Add")]
        public async Task<OperationResult> CreateCart(AddItemCommand command)
        {
            var result = await _cartFacade.AddItem(command);
            return result;
        }

        [HttpPost("Clear")]
        public async Task<OperationResult> ClearCart(ClearCartCommand command)
        {
            var result = await _cartFacade.Clear(command);
            return result;
        }

        [HttpPost("MergeGuestCart")]
        public async Task<OperationResult> MegeGuestCart(MergeGuestCartCommand command)
        {
            var result = await _cartFacade.MergeGuestCart(command);
            return result;
        }

        [HttpPost("RemoveItemFromCart")]
        public async Task<OperationResult> RemoveItemFromCart(RemoveItemCommand command)
        {
            var result = await _cartFacade.RemoveItem(command);
            return result;
        }

        [HttpGet("getByCustomerId/{customerId}")]
        public async Task<IActionResult> GetCartByCustomerId(Guid customerId)
        {
            var result = await _cartFacade.GetByCustomerId(customerId);
            return Ok(result);
        }

        [HttpGet("getByGuestId/{guestId}")]
        public async Task<IActionResult> GetCartByGuestId(string guestId)
        {
            var result = await _cartFacade.GetByGuestId(guestId);
            return Ok(result);
        }

        [HttpGet("GetByFilter")]
        public async Task<IActionResult> GetByFilter(CartFilterParam filterParam)
        {
            var result = await _cartFacade.GetByFilter(filterParam);
            return Ok(result);
        }

    }
}
