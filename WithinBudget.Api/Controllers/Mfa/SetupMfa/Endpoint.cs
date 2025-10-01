using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QRCoder.Core;
using WithinBudget.Api.Data.Entities;

namespace WithinBudget.Api.Controllers.Mfa.SetupMfa;

[ApiController]
[Authorize]
[Route("mfa")]
public class SetupMfa(UserManager<User> userManager, IConfiguration config) : ControllerBase
{
    [HttpPost("setup")]
    public async Task<IActionResult> PostAsync([FromBody] string email)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user == null)
        {
            return Unauthorized();
        }

        await userManager.ResetAuthenticatorKeyAsync(user);

        var key = await userManager.GetAuthenticatorKeyAsync(user);
        var uri = $"otpauth://totp/{config["jwt:issuer"]}:{user.Email}?secret={key}&issuer={config["jwt:issuer"]}&digits=6";

        var qrGenerator = new QRCodeGenerator();
        var qrCodeData = qrGenerator.CreateQrCode(uri, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new PngByteQRCode(qrCodeData);
        var qrBytes = qrCode.GetGraphic(20);
        
        var qrBase64 = $"data:image/png;base64,{Convert.ToBase64String(qrBytes)}";
        
        return Ok(new { Key = key, Uri = uri, QrCode = qrBase64 });
    }
}