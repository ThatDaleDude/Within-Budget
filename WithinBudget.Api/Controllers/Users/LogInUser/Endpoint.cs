using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WithinBudget.Api.Data.Entities;
using WithinBudget.Api.Infrastructure.Authentication;
using WithinBudget.Shared.Login;

namespace WithinBudget.Api.Controllers.Users.LogInUser;

[ApiController]
[Route("user")]
public class LogInUser(UserManager<User> userManager, IConfiguration config) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> PostAsync([FromBody] CommandCriteria command)
    {
        var user = await userManager.FindByEmailAsync(command.Email);

        if (user == null)
        {
            return Unauthorized("Invalid email or password.");
        }

        var isPasswordValid = await userManager.CheckPasswordAsync(user, command.Password);
        
        if (await userManager.IsLockedOutAsync(user))
        {
            return Unauthorized("User is temporarily locked out.");
        }
        
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

        var token = JwtTokenGenerator.GenerateToken(user, config["Jwt:Key"]!, config["Jwt:Issuer"]!);

        return Ok(new LoginUserResponse { Token = token });
    }
}