using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WithinBudget.Api.Data.Entities;
using WithinBudget.Api.Infrastructure.Authentication;

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
        
        if (!isPasswordValid)
        {
            return Unauthorized("Invalid email or password.");
        }

        var token = JwtTokenGenerator.GenerateToken(user, config["Jwt:Key"]!, config["Jwt:Issuer"]!);

        return Ok(token);
    }
}