using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using WithinBudget.Shared;

namespace WithinBudget.Pages.Auth;

public partial class Register : ComponentBase
{
    private readonly RegisterModel _model = new();
    private Dictionary<string, string[]> _errorMessages = new();
    private bool _showPassword;

    private async Task AttemptRegister()
    {
        var response = await Http.PostAsJsonAsync("/user/create", _model);

        if (response.IsSuccessStatusCode)
        {
            Navigation.NavigateTo("/");
            return;
        }
        
        var content = await response.Content.ReadAsStreamAsync();

        try
        {
            var apiError = await JsonSerializer.DeserializeAsync<ApiError>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            _errorMessages = apiError?.Errors ?? [];
        }
        catch
        {
            _errorMessages = new Dictionary<string, string[]>
            {
                { "", ["An unknown error occurred"] }
            };
        }
    }
    
    private void TogglePasswordVisibility() => _showPassword = !_showPassword;
}