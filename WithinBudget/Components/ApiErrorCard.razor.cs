using Microsoft.AspNetCore.Components;
using WithinBudget.Shared;

namespace WithinBudget.Components;

public partial class ApiErrorCard : ComponentBase
{
    [Parameter] public ApiError ApiError { get; set; } = new();
}