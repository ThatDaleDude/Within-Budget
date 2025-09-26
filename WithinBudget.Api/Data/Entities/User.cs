using Microsoft.AspNetCore.Identity;

namespace WithinBudget.Api.Data.Entities;

public class User : IdentityUser<Guid>
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
}