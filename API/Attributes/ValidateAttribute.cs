using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Utils;

namespace API.Attributes
{
    public class ValidateAttribute<DTO>: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var validatorService = context.HttpContext.RequestServices.GetService<ValidatorService>() ?? throw new ArgumentNullException();
            var modelIsValid = validatorService.Validate(context.ActionArguments.Values.OfType<DTO>().Single(), context.ModelState).GetAwaiter().GetResult();
            if(!modelIsValid)
            {
                context.Result = new UnprocessableEntityObjectResult(new { errors = context.ModelState.GetErrors() });
            }
            base.OnActionExecuting(context);
        }
    }
}
