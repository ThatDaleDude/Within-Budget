using FluentValidation;

namespace WithinBudget.Api.Controllers.Users.LogInUser;

public class CommandValidator : AbstractValidator<CommandCriteria>
{
    public CommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email address is required.")
            .EmailAddress()
            .WithMessage("Please enter a valid email address.");
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Please enter a password.");
    }
}