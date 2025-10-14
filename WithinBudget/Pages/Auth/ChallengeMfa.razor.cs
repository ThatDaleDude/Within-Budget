using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using WithinBudget.Infrastructure;
using WithinBudget.Shared;
using WithinBudget.Shared.Login;

namespace WithinBudget.Pages.Auth;

public partial class ChallengeMfa : ComponentBase
{
    [Inject] private HttpClient Http { get; set; } = null!;
    [Inject] private AuthenticationStateProvider AuthStateProvider { get; set; } = null!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;

    private string? _confirmationError;
    
    [Parameter] public Guid UserId { get; set; }
    private readonly MfaChallengeModel _model = new();

    protected override void OnInitialized() => _model.UserId = UserId;

    private async Task SubmitChallenge()
    {
        var response = await Http.PostAsJsonAsync("/mfa/challenge", _model);

        if (!response.IsSuccessStatusCode)
        {
            var apiError = await response.Content.ReadFromJsonAsync<ApiError>();
            _confirmationError = apiError?.Error;
            return;
        }

        var customProvider = AuthStateProvider as CustomAuthStateProvider;
        customProvider?.MarkUserAsAuthenticated();
        
        Navigation.NavigateTo("/Profile", forceLoad: true);
    }
}