using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EShop.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Area("admin")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] 
    public abstract class AdminBaseController : ControllerBase
    {
    }
}
