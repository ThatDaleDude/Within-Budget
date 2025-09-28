using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using WithinBudget.Infrastructure;
using WithinBudget.Shared;

namespace WithinBudget.Pages.Auth;

public partial class Login : ComponentBase
{
    private readonly LoginModel _model = new();
    private ApiError _errorMessages = new();


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
            var errors = JsonSerializer.Deserialize<ApiError>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            _errorMessages = errors ?? new ApiError();
        }
        catch (JsonException)
        {
            _errorMessages = new ApiError
            {
                Errors = new Dictionary<string, string[]>
                {
                    { "", [content] }
                }
            };
        }
    }
}