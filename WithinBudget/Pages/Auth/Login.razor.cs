using System.Net.Http.Json;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using WithinBudget.Infrastructure;
using WithinBudget.Shared;
using WithinBudget.Shared.Login;

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
            var model = await response.Content.ReadFromJsonAsync<LoginUserResponse>();
            await SetTokenAndLogin(model);

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

    private async Task SetTokenAndLogin(LoginUserResponse? model)
    {
        if (model == null)
        {
            return;
        }

        if (model.ChallengeMfa)
        {
            Navigation.NavigateTo($"/ChallengeMfa/{model.UserId}");
            return;
        }
        
        // TODO: Store this differently. It's bad practice to store sensitive data in local storage.
        await LocalStorage.SetItemAsStringAsync("authToken", model.Token!);
        
        var customProvider = AuthStateProvider as CustomAuthStateProvider;
        customProvider?.MarkUserAsAuthenticated(model.Token!);
        
        Navigation.NavigateTo("/Profile", forceLoad: true);
    }
    
    private void TogglePasswordVisibility() => _showPassword = !_showPassword;
}