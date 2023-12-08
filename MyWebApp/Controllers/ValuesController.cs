using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MyWebApp.Controllers;

[ApiController]
[Route("[controller]")]
public class ValuesController(ILogger<ValuesController> logger) : ControllerBase
{
    [HttpGet]
    public ActionResult Get()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRole = User.FindFirstValue(ClaimTypes.Role);
        logger.LogInformation("User ID = {userId}, Role = {userRole}", userId, userRole);
        return Ok(userRole == "Admin" ? ["value1", "value2"] : Array.Empty<string>());
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