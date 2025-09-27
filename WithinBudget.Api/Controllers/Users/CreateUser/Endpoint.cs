using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WithinBudget.Api.Controllers.Users.CreateUser;

[ApiController]
[Route("user")]
public class CreateUser(UserManager<Data.Entities.User> userManager) : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> PostAsync([FromBody] CommandCriteria command)
    {
        var user = new Data.Entities.User
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