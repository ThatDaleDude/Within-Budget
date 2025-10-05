namespace WithinBudget.Shared.Login;

public class MfaChallengeModel
{
    public Guid UserId { get; set; }
    public string? Code { get; set; }
}