using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace IdentityAppTest.Controllers;

public class HomeController : Controller
{
    private class StubObj
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    [HttpGet("api/v1/[action]")]
    [Authorize]
    public IActionResult Index()
    {
        var pers = new StubObj();
        pers.Id = 12345;

        var response = new ContentResult();
        response.StatusCode = StatusCodes.Status200OK;
        response.ContentType = "application/json";

        var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

        if (role != null)
        {
            if (role == "User")
                pers.Name = "Object for User";
            else if (role == "Moderator")
                pers.Name = "Object for Moderator";

            response.Content = JsonSerializer.Serialize(pers);
            return response;
        }

        response.StatusCode = StatusCodes.Status401Unauthorized;
        return response;
    }
}
