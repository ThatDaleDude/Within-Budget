namespace WithinBudget.Shared;

public class ApiError
{
    public Dictionary<string, string[]> Errors { get; init; } = [];
    public string? Error { get; init; }
}