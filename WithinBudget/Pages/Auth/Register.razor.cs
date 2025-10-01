using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using WithinBudget.Shared;

namespace WithinBudget.Pages.Auth;

public partial class Register : ComponentBase
{
    private readonly RegisterModel _model = new();
    private ApiError _errorMessages = new();
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
            var errors = await JsonSerializer.DeserializeAsync<ApiError>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            _errorMessages = errors ?? new ApiError();
        }
        catch
        {
            _errorMessages = new ApiError
            {
                Errors = new Dictionary<string, string[]>
                {
                    { "General", ["An unknown error occurred"] }
                }
            };
        }
    }
    
    private void TogglePasswordVisibility() => _showPassword = !_showPassword;
}