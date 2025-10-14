using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WithinBudget.Api.Data.Entities;
using WithinBudget.Shared;

namespace WithinBudget.Api.Controllers.Users.GetUser;

[ApiController]
[Authorize]
[Route("user")]
public class GetUser(UserManager<User> userManager) : ControllerBase
{
    [HttpGet("profile")]
    public async Task<IActionResult> GetAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
        {
            return Unauthorized();
        }

        var user = await userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(new UserModel
        {
            Id    = user.Id,
            Email = user.Email
        });
    }
}