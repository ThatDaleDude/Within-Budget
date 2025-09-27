using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WithinBudget.Api.Infrastructure.Validation;

public class ValidationFilter<T>(IValidator<T> validator) : IAsyncActionFilter
    where T : class
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var argument = context.ActionArguments.Values.OfType<T>().FirstOrDefault();

        if (argument == null)
        {
            await next();
            return;
        }

        var result = await validator.ValidateAsync(argument);

        if (!result.IsValid)
        {
            var errors = result.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(x => x.Key, x => x.Select(y => y.ErrorMessage).ToArray());

            context.Result = new BadRequestObjectResult(new { errors });
            return;
        }
        
        await next();
    }
}