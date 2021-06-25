using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace MyWebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger<ValuesController> _logger;

        public ValuesController(ILogger<ValuesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Get()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            _logger.LogInformation($"User ID = {userId}, Role = {userRole}");
            if (userRole != "Admin")
            {
                return Ok(new string[] { });
            }
            return Ok(new[] { "value1", "value2" });
        }

        [HttpGet("{id:int}")]
        public ActionResult GetValueById(int id)
        {
            var lang = Request.Headers["lang"].ToString();
            var value = lang switch
            {
                "eng" => "value" + id,
                _ => id.ToString()
            };
            return Ok(value);
        }
    }
}
