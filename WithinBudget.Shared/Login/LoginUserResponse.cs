namespace WithinBudget.Shared.Login;

public class LoginUserResponse
{
    public bool ChallengeMfa { get; init; }
    public Guid UserId { get; init; }
}