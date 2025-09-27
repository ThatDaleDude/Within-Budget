using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using WithinBudget.Infrastructure;
using WithinBudget.Shared;

namespace WithinBudget.Pages.Auth;

public partial class Login : ComponentBase
{
    private readonly LoginModel _model = new();
    private string _errorMessage = "";

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
        }
        else
        {
            _errorMessage = "Invalid login attempt.";
        }
    }
}