namespace WithinBudget.Api.Controllers.Users.LogInUser;

public class CommandCriteria
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}