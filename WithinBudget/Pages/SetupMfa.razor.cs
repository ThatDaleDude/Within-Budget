using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using WithinBudget.Shared;

namespace WithinBudget.Pages;

public partial class SetupMfa : ComponentBase
{
    [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;
    [Inject] private HttpClient Http { get; set; } = null!;
    
    private SetupMfaResponse? _mfaResponse;
    private bool? _mfaEnabled;
    private string _otpCode = "";
    private string? _confirmationError;
    private bool _verified;
    private string _userEmail = "";
    
    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateTask;
        var user = authState.User;
        
        if (user.Identity?.IsAuthenticated != true)
        {
            return;
        }
        
        _userEmail = user.FindFirst(x => x.Type == "email")?.Value ?? "";
        var response = await Http.GetAsync($"mfa/status/{_userEmail}");

        _mfaEnabled = await response.Content.ReadFromJsonAsync<bool>();
    }
    
    private async Task GenerateQrCode()
    {
        var response = await Http.PostAsJsonAsync("/mfa/setup", _userEmail);

        if (!response.IsSuccessStatusCode)
        {
            return;
        }
        
        _mfaResponse = await response.Content.ReadFromJsonAsync<SetupMfaResponse>();
    }

    private async Task VerifyMfa()
    {
        if (string.IsNullOrWhiteSpace(_otpCode))
        {
            return;
        }

        var response = await Http.PostAsync($"mfa/confirm/{_otpCode}", null);

        if (response.IsSuccessStatusCode)
        {
            _verified = true;
            return;
        }
        
        var apiError = await response.Content.ReadFromJsonAsync<ApiError>();
        _confirmationError = apiError?.Error;
    }

    private async Task DisableMfa()
    {
        var response = await Http.PostAsJsonAsync("mfa/disable", _userEmail);
        
        if (response.IsSuccessStatusCode)
        {
            _mfaEnabled = false;
        }
    }
}