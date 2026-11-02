using _01.Domain.Entities.Aggregates.AdminAgg;
using _02.Application.CommentAgg.Commands.AddComment;
using _02.Application.CommentAgg.Commands.DeleteComment;
using _02.Application.ProductAgg.Commands.AddImage;
using _02.Application.ProductAgg.Commands.ChangePrice;
using _02.Application.ProductAgg.Commands.CreateProduct;
using _02.Application.ProductAgg.Commands.IncreaseStock;
using _02.Application.ProductAgg.Commands.RemoveImage;
using EShop.Facade.Product;
using EShop.Query.CommentAgg.DTOs.Product;
using EShop.Query.CustomerAgg.DTOs;
using EShop.Query.ProductAgg.DTOs;
using EShop.Query.ProductAgg.DTOs.Admin;
using EShop.Query.ProductAgg.DTOs.Customer;
using EShop.Query.ProductAgg.DTOs.Details;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EShop.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductFacade _productFacade;

        public ProductsController(IProductFacade productFacade)
        {
            _productFacade = productFacade;
        }

        [HttpPost("images")] // POST /api/products/{productId}/images
        [Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddImage([FromForm] AddProductImageCommand command)
        {
            var result = await _productFacade.AddImage(command);

            return StatusCode(StatusCodes.Status201Created, result);
        }

        [HttpPut("{productId:guid}/changePrice")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePrice(
            Guid productId,
            [FromBody] ChangePriceRequestDto request)
        {

            var command = new ChangePriceCommand(productId, request.NewPrice);
            var result = await _productFacade.ChangePrice(command);

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
        {
            var result = await _productFacade.Add(command);

            return StatusCode(StatusCodes.Status201Created, result);
        }

        [HttpPut("{productId:guid}/increaseStock")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> IncreaseStock(
            Guid productId,
            [FromBody] IncreaseStockRequestDto request)
        {

            var command = new IncreaseStockCommand(productId, request.Quantity);
            var result = await _productFacade.IncreaseStock(command);

            return Ok(result);
        }

        [HttpDelete("{productId}/images/{imageId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid productId,
            [FromQuery] DeleteImageProductRequestDto dto)
        {
            var result = await _productFacade.RemoveImage(
                new RemoveProductImageCommand(productId,dto.ImageId));
            return Ok(result);
        }

        [HttpGet("{productId:guid}/stock")]
        [Authorize]
        [ProducesResponseType(typeof(ProductStockDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<ProductStockDto?>> CheckProductStock(Guid productId,
            [FromQuery] CheckStockRequestDto dto )
        {
            var result = await _productFacade.CheckStock(productId,dto.RequiredCount);
            if (result is null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("{productId:guid}")]
        [ProducesResponseType(typeof(ProductDetailsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDetailsDto?>> GetById(Guid productId)
        {
            var result = await _productFacade.GetById(productId);
            if(result is null)
                return NotFound();
            return Ok(result);
        }


        [HttpGet("adminFilter")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ProductAdminFilterResult), StatusCodes.Status200OK)]
        public async Task<ActionResult<ProductAdminFilterResult>> GetAllForAdmin(
            [FromQuery] ProductAdminFilterParams filterParams)
        {
            var result = await _productFacade.GetForAdmin(filterParams);
            return Ok(result);
        }

        [HttpGet("customerFilter")]
        [ProducesResponseType(typeof(ShopProductFilterResult), StatusCodes.Status200OK)]
        public async Task<ActionResult<ShopProductFilterResult>> GetAllForCustomer(
            [FromQuery] ShopProductFilterParams filterParams)
        {
            var result = await _productFacade.GetForCustomer(filterParams);
            return Ok(result);
        }


    }

    public record ChangePriceRequestDto(decimal NewPrice);
    public record IncreaseStockRequestDto(int Quantity);
    public record DeleteImageProductRequestDto(Guid ImageId);
    public record CheckStockRequestDto(int RequiredCount);
}
