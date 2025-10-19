using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WithinBudget.Api.Controllers.Shared;
using WithinBudget.Api.Data.Entities;
using WithinBudget.Shared.Login;

namespace WithinBudget.Api.Controllers.Users.LogInUser;

[ApiController]
[Route("user")]
public class LogInUser(UserManager<User> userManager, IConfiguration config) : LoginController
{
    [HttpPost("login")]
    public async Task<IActionResult> PostAsync([FromBody] CommandCriteria command)
    {
        var user = await userManager.FindByEmailAsync(command.Email);

        if (user == null)
        {
            return Unauthorized("Invalid email or password.");
        }

        if (await userManager.IsLockedOutAsync(user))
        {
            return Unauthorized("User is temporarily locked out.");
        }

        var isPasswordValid = await userManager.CheckPasswordAsync(user, command.Password);
        
        if (!isPasswordValid)
        {
            await userManager.AccessFailedAsync(user);
            return Unauthorized("Invalid email or password.");
        }

        if (user.TwoFactorEnabled)
        {
            return Ok(new LoginUserResponse
            {
                ChallengeMfa = true,
                UserId = user.Id
            });
        }

        GenerateAuthToken(user, config["Jwt:Key"]!, config["Jwt:Issuer"]!);
        return Ok(new LoginUserResponse { UserId = user.Id });
    }
}