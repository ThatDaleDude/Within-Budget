using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WithinBudget.Api.Controllers.Users.LogoutUser;

[ApiController]
[Authorize]
[Route("user/logout")]
public class LogoutUser : ControllerBase
{
    [HttpPost]
    public IActionResult PostAsync()
    {
        Response.Cookies.Delete("AuthToken");
        return Ok();
    }
}