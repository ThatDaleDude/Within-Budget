using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WithinBudget.Api.Controllers.Shared;
using WithinBudget.Api.Data.Entities;
using WithinBudget.Shared.Login;

namespace WithinBudget.Api.Controllers.Mfa.ChallengeMfa;

[ApiController]
[Route("mfa")]
public class ChallengeMfa(UserManager<User> userManager, IConfiguration config) : LoginController
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
        
        GenerateAuthToken(user, config["Jwt:Key"]!, config["Jwt:Issuer"]!);
        return Ok(new LoginUserResponse { UserId = user.Id });
    }
}