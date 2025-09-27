using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WithinBudget.Api.Data.Entities;

namespace WithinBudget.Api.Infrastructure.Authentication;

public static class JwtTokenGenerator
{
    public static string GenerateToken(User user, string key, string issuer, int expiresMinutes = 60)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!)
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer, issuer, claims, expires: DateTime.UtcNow.AddMinutes(expiresMinutes), signingCredentials: credentials);
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}