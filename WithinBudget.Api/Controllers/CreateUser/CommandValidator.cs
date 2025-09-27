using FluentValidation;
using WithinBudget.Api.Infrastructure.Validation;

namespace WithinBudget.Api.Controllers.CreateUser;

public class CommandValidator : AbstractValidator<CommandCriteria>
{
    public CommandValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("A valid email address is required.")
            .MaximumLength(256)
            .WithMessage("A valid email address must be less than 256 characters.");
        
        RuleFor(x => x.FirstName)
            .MaximumLength(50)
            .WithMessage("First name cannot exceed 50 characters.");
        
        RuleFor(x => x.LastName)
            .MaximumLength(50)
            .WithMessage("Last name cannot exceed 50 characters.");

        RuleFor(x => x.Password)
            .ValidPassword();
    }
}