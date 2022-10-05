using Microsoft.AspNetCore.Mvc;
using ProjectA.Models;

namespace ProjectA.Controllers
{
    [ApiController]
    [Route("api")]
    public class BaseController : Controller
    {
        private readonly ILogger<BaseController> _logger;

        public BaseController(ILogger<BaseController> logger)
        {
            _logger = logger;
        }

        [HttpPost("send")]
        public IActionResult Index(Client value)
        {
            _logger.LogInformation($"Recieved new code: {value.Id}");

            return Ok();
        }
    }
}
