using _02.Application.CommentAgg.Commands.AddComment;
using _02.Application.CommentAgg.Commands.DeleteComment;
using _02.Application.CustomerAgg.Commands.AddCustomerAddress;
using _02.Application.CustomerAgg.Commands.ChangeDefaultAddress;
using _02.Application.CustomerAgg.Commands.EditCustomerAddress;
using _02.Application.CustomerAgg.Commands.RegisterCustomer;
using _02.Application.CustomerAgg.Commands.RemoveCustomerAddress;
using _02.Application.CustomerAgg.Commands.UpdateCustomerProfile;
using EShop.Facade.Customer;
using EShop.Query.CommentAgg.DTOs.Product;
using EShop.Query.CustomerAgg.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EShop.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerFacade _customerFacade;

        public CustomersController(ICustomerFacade customerFacade)
        {
            _customerFacade = customerFacade;
        }

        [HttpPost("address")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddAddress([FromBody]AddCustomerAddressCommand command)
        {
            var result = await _customerFacade.AddAddress(command);
            return StatusCode(StatusCodes.Status201Created, result);
        }

        [HttpPut("default/{postalCode}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangeDefaultAddress(
            string postalCode)
        {
            var command = new ChangeDefaultAddressCommand(postalCode);
            var result = await _customerFacade.ChangeDefaultAddress(command);

            return Ok(result);
        }

        [HttpPut("{targetPostalCode}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EditAddress(string targetPostalCode
            ,[FromBody]EditAddressDto dto)
        {
            var command = new EditCustomerAddressCommand(dto.ReceiverName,dto.PhoneNumber
            ,targetPostalCode,dto.Title,dto.Province,dto.City,dto.Street,dto.Plaque,dto.PostalCode);

            var result = await _customerFacade.EditAddress(command);
            return Ok(result);
        }

        [HttpDelete("{postalCode}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(string postalCode)
        {
            var result = await _customerFacade.RemoveAddress(
                new RemoveCustomerAddressCommand(postalCode));
            return Ok(result);
        }

        //[HttpPost("register")]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> Register([FromBody] RegisterCustomerCommand command)
        //{
        //    var result = await _customerFacade.Register(command);
        //    return StatusCode(StatusCodes.Status201Created, result);
        //}

        [HttpPut("profile")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProfile([FromBody] EditProfileDto dto)
        {
            var command = new UpdateCustomerProfileCommand(dto.Name,dto.Family);

            var result = await _customerFacade.UpdateProfile(command);
            return Ok(result);
        }

        [HttpGet("addresses/{customerId:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(IReadOnlyList<AddressDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<AddressDto>>> GetAddresses(
            Guid customerId)
        {
            var result = await _customerFacade.GetAddresses(customerId);
            return Ok(result);
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(CustomerFilterResult), StatusCodes.Status200OK)]
        public async Task<ActionResult<CustomerFilterResult>> GetAllForAdmin(
            [FromQuery] CustomerFilterParams filterParams)
        {
            var result = await _customerFacade.GetCustomerLookupForAdmin(filterParams);
            return Ok(result);
        }

        [HttpGet("profile/{customerId:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(CustomerProfileDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<CustomerProfileDto>> GetProfile(
            Guid customerId)
        {
            var result = await _customerFacade.GetProfile(customerId);
            return Ok(result);
        }


    }
    public record EditAddressDto(
        string ReceiverName,
        string PhoneNumber,
        string Title,
        string Province,
        string City,
        string Street,
        string Plaque,
        string PostalCode);
    public record EditProfileDto(string Name, string Family);
}

