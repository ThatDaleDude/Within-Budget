namespace WithinBudget.Api.Controllers.Mfa.ChallengeMfa;

public class CommandCriteria
{
    public Guid UserId { get; set; }
    public string Code { get; set; } = "";
}