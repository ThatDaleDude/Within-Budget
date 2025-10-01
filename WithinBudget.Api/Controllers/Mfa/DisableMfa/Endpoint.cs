using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WithinBudget.Api.Data.Entities;

namespace WithinBudget.Api.Controllers.Mfa.DisableMfa;

[ApiController]
[Authorize]
[Route("mfa")]
public class DisableMfa(UserManager<User> userManager) : ControllerBase
{
    [HttpPost("disable")]
    public async Task<IActionResult> PostAsync([FromBody] string email)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user == null)
        {
            return Unauthorized();
        }

        var result = await userManager.SetTwoFactorEnabledAsync(user, false);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                Console.WriteLine(error.Description);
            }

            return BadRequest(new { Error = "Failed to disable MFA" });
        }
        
        await userManager.ResetAuthenticatorKeyAsync(user);
        return Ok();
    }
}