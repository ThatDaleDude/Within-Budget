using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WithinBudget.Api.Data.Entities;

namespace WithinBudget.Api.Controllers.CreateUser;

[ApiController]
[Route("users")]
public class CreateUser(UserManager<User> userManager) : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> PostAsync([FromBody] CommandCriteria command)
    {
        var user = new User
        {
            Email     = command.Email,
            UserName  = command.Email,
            FirstName = command.FirstName,
            LastName  = command.LastName
        };

        var result = await userManager.CreateAsync(user, command.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok(new { user.Id, user.FirstName, user.Email });
    }
}