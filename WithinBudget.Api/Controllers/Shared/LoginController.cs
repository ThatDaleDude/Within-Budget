using Microsoft.AspNetCore.Mvc;
using WithinBudget.Api.Data.Entities;
using WithinBudget.Api.Infrastructure.Authentication;

namespace WithinBudget.Api.Controllers.Shared;

public abstract class LoginController : ControllerBase
{
    protected void GenerateAuthToken(User user, string key, string issuer)
    {
        var token = JwtTokenGenerator.GenerateToken(user, key, issuer);
        
        Response.Cookies.Append("AuthToken", token, new CookieOptions
        {
            HttpOnly = true,
            Secure   = true,
            SameSite = SameSiteMode.Strict,
            Expires  = DateTime.UtcNow.AddHours(1)
        });
    }
}