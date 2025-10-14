using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using WithinBudget.Shared;

namespace WithinBudget.Infrastructure;

public class CustomAuthStateProvider(HttpClient client, ILogger<CustomAuthStateProvider> logger) : AuthenticationStateProvider
{
    private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());
    
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var response = await client.GetAsync("/user/profile");

        if (!response.IsSuccessStatusCode)
        {
            return new AuthenticationState(_anonymous);
        }

        try
        {
            var userModel = await response.Content.ReadFromJsonAsync<UserModel>();

            if (userModel == null)
            {
                return new AuthenticationState(_anonymous);
            }
            
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userModel.Id.ToString()),
                new Claim(ClaimTypes.Email, userModel.Email ?? "")
            };
            
            var identity = new ClaimsIdentity(claims, "serverAuth");
            var user = new ClaimsPrincipal(identity);
            
            return new AuthenticationState(user);
        }
        catch
        {
            logger.LogError("Failed to parse user profile");
            return new AuthenticationState(_anonymous);
        }
    }

    public void MarkUserAsAuthenticated() => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

    public async Task MarkUserAsLoggedOut()
    {
        await client.PostAsync("user/logout", null);
        
        var authState = Task.FromResult(new AuthenticationState(_anonymous));
        NotifyAuthenticationStateChanged(authState);
    }
}