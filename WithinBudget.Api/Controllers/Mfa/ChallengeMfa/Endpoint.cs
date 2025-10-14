using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WithinBudget.Api.Data.Entities;
using WithinBudget.Api.Infrastructure.Authentication;

namespace WithinBudget.Api.Controllers.Mfa.ChallengeMfa;

[ApiController]
[Route("mfa")]
public class ChallengeMfa(UserManager<User> userManager, IConfiguration config) : ControllerBase
{
    [HttpPost("challenge")]
    public async Task<IActionResult> PostAsync([FromBody] CommandCriteria command)
    {
        var user = await userManager.FindByIdAsync(command.UserId.ToString());

        if (user == null)
        {
            return Unauthorized();
        }

        var isValid = await userManager.VerifyTwoFactorTokenAsync(
            user,
            TokenOptions.DefaultAuthenticatorProvider,
            command.Code);

        if (!isValid)
        {
            return BadRequest(new { Error = "Invalid code" });
        }
        
        var token = JwtTokenGenerator.GenerateToken(user, config["Jwt:Key"]!, config["Jwt:Issuer"]!);
        
        Response.Cookies.Append("AuthToken", token, new CookieOptions
        {
            HttpOnly = true,
            Secure   = true,
            SameSite = SameSiteMode.Strict,
            Expires  = DateTime.UtcNow.AddHours(1)
        });

        return Ok();
    }
}