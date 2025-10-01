using System.Net.Http.Json;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using WithinBudget.Infrastructure;
using WithinBudget.Shared;

namespace WithinBudget.Pages.Auth;

public partial class Login : ComponentBase
{
    [Inject] private HttpClient Http { get; set; } = null!;
    [Inject] private ILocalStorageService LocalStorage { get; set; } = null!;
    [Inject] private AuthenticationStateProvider AuthStateProvider { get; set; } = null!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;
    
    private readonly LoginModel _model = new();
    private Dictionary<string, string[]> _errorMessages = new();
    private bool _showPassword;

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (user.Identity?.IsAuthenticated == true)
        {
            Navigation.NavigateTo("/Profile");
        }
    }

    private async Task AttemptLogin()
    {
        var response = await Http.PostAsJsonAsync("/user/login", _model);

        if (response.IsSuccessStatusCode)
        {
            var token = await response.Content.ReadAsStringAsync();
            await LocalStorage.SetItemAsStringAsync("authToken", token);

            var customProvider = AuthStateProvider as CustomAuthStateProvider;
            customProvider?.MarkUserAsAuthenticated(token);
            Navigation.NavigateTo("/Profile", forceLoad: true);

            return;
        }
        
        var content = await response.Content.ReadAsStringAsync();

        try
        {
            var apiError = JsonSerializer.Deserialize<ApiError>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            _errorMessages = apiError?.Errors ?? [];
        }
        catch (JsonException)
        {
            _errorMessages = new Dictionary<string, string[]>
            {
                { "", [content] }
            };
        }
    }
    
    private void TogglePasswordVisibility() => _showPassword = !_showPassword;
}