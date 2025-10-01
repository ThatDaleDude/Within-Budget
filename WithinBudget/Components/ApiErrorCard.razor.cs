using Microsoft.AspNetCore.Components;

namespace WithinBudget.Components;

public partial class ApiErrorCard : ComponentBase
{
    [Parameter] public Dictionary<string, string[]> Errors { get; set; } = [];
}