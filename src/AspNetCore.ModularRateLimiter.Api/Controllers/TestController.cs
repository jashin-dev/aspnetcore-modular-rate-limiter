using AspNetCore.ModularRateLimiter.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.ModularRateLimiter.Api.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var result = new ApiResponse
            {
                Success = true,
                Message = "Request processed successfully",
                Timestamp = DateTime.Now,
                Path = HttpContext.Request.Path
            };
            return Ok(result);
        }
    }
}
