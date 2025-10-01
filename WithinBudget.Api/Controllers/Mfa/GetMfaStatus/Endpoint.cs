using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WithinBudget.Api.Data.Entities;

namespace WithinBudget.Api.Controllers.Mfa.GetMfaStatus;

[ApiController]
[Authorize]
[Route("mfa")]
public class GetMfaStatus(UserManager<User> userManager) : ControllerBase
{
    [HttpGet("status/{email}")]
    public async Task<IActionResult> GetAsync([FromRoute] string email)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user == null)
        {
            return Unauthorized();
        }
        
        return Ok(user.TwoFactorEnabled);
    }
}