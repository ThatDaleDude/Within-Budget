using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WithinBudget.Api.Data.Entities;

namespace WithinBudget.Api.Controllers.Mfa.ConfirmMfaSetup;

[ApiController]
[Authorize]
[Route("mfa")]
public class ConfirmMfaSetup(UserManager<User> userManager, IConfiguration config) : ControllerBase
{
    [HttpPost("confirm/{code}")]
    public async Task<IActionResult> PostAsync([FromRoute] string code)
    {
        var user = await userManager.GetUserAsync(User);

        if (user == null)
        {
            return Unauthorized();
        }

        var isValid = await userManager.VerifyTwoFactorTokenAsync(
            user,
            TokenOptions.DefaultAuthenticatorProvider,
            code);

        if (!isValid)
        {
            return BadRequest(new { Error = "Invalid code" });
        }

        user.TwoFactorEnabled = true;
        await userManager.UpdateAsync(user);

        return Ok();
    }
}