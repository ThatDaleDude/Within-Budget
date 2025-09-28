using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WithinBudget.Shared;

namespace WithinBudget.Api.Infrastructure.Validation;

public class ValidationFilter(IServiceProvider provider) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {

        foreach (var arg in context.ActionArguments.Values)
        {
            if (arg == null) continue;

            var validatorType = typeof(IValidator<>).MakeGenericType(arg.GetType());
            var validator = provider.GetService(validatorType);

            if (validator is not IValidator validatorObj)
            {
                continue;
            }
            
            
            var validationContext = new ValidationContext<object>(arg);
            var result = await validatorObj.ValidateAsync(validationContext);

            if (result.IsValid)
            {
                continue;
            }
            
            var errors = result.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );

            context.Result = new BadRequestObjectResult(new ApiError { Errors = errors });
            return;
        }

        await next();
    }
}