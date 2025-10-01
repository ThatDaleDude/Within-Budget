using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using WithinBudget.Shared;

namespace WithinBudget.Pages;

public partial class SetupMfa : ComponentBase
{
    [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;
    private SetupMfaResponse? _mfaResponse;
    
    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateTask;
        var user = authState.User;

        if (user.Identity?.IsAuthenticated != true)
        {
            return;
        }
        
        var email = user.FindFirst(x => x.Type == "email")?.Value;
        var response = await Http.PostAsJsonAsync("/mfa/setup", email);

        if (!response.IsSuccessStatusCode)
        {
            return;
        }
        
        _mfaResponse = await response.Content.ReadFromJsonAsync<SetupMfaResponse>();
    }
}