namespace WithinBudget.Shared.Login;

public class LoginUserResponse
{
    public string? Token { get; init; }
    public bool ChallengeMfa { get; init; }
    public Guid UserId { get; init; }
}