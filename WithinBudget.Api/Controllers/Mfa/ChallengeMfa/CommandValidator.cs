using FluentValidation;

namespace WithinBudget.Api.Controllers.Mfa.ChallengeMfa;

public class CommandValidator : AbstractValidator<CommandCriteria>
{
    public CommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Code is required.");
    }
}