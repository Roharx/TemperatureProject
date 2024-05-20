using exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace api.Filters;

public class ValidateAccountIdAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var accountId = context.HttpContext.Items["Account"] as string;
        if (string.IsNullOrEmpty(accountId))
        {
            throw new Exceptions.InvalidTokenException();
        }
        
        base.OnActionExecuting(context);
    }
}