using EShop.Query.CartAgg.GetCartByCustomerId;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EShop.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private IMediator _mediator;

        public TestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{customerId:guid}")]
        public async Task<IActionResult> GetCartByCustomerId([FromRoute]Guid customerId)
        {
            var result = await _mediator.Send(new GetCartByCustomerIdQuery(customerId));

            if (result == null)
            {
                return NotFound(new { message = "سبد خریدی برای این مشتری یافت نشد." });
            }
            return Ok(result);
        }
    }
}
