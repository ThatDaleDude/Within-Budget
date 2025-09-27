using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using WithinBudget.Shared;

namespace WithinBudget.Pages.Auth;

public partial class Register : ComponentBase
{
    private readonly RegisterModel _model = new();
    private string _errorMessage = "";

    private async Task AttemptRegister()
    {
        _errorMessage = string.Empty;

        var response = await Http.PostAsJsonAsync("/user/create", _model);

        if (response.IsSuccessStatusCode)
        {
            Navigation.NavigateTo("/login");
        }
        else
        {
            _errorMessage = "Registration failed.";
        }
    }
}