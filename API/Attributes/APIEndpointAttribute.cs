using Common.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace API.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class APIEndpointAttribute : ActionFilterAttribute, IOrderedFilter
    {
        private readonly HttpMethodTypes? ActualHttpMethod = null;
        private readonly bool SendRawResult = false;

        public APIEndpointAttribute()
        {
            Order = 1;
        }
        public APIEndpointAttribute(bool sendRawResult)
        {
            SendRawResult = sendRawResult;
            Order = 0;
        }
        public APIEndpointAttribute(HttpMethodTypes actualHttpMethod)
        {
            ActualHttpMethod = actualHttpMethod;
            Order = 0;
        }
        public APIEndpointAttribute(HttpMethodTypes actualHttpMethod, bool sendRawResult) 
        { 
            ActualHttpMethod = actualHttpMethod;
            SendRawResult = sendRawResult;
            Order = 0;
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            
        }
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.ModelState.IsValid
                && context.Result is not UnprocessableEntityObjectResult
                && context.Result is not BadRequestObjectResult
                && (context.Filters.Count(f => f is APIEndpointAttribute) == 1 || Order == 0))
            {
                var resultObj = (context.Result as ObjectResult)!.Value;
                var method = ActualHttpMethod ?? context.HttpContext.Request.GetRequestMethodType();
                switch (method)
                {
                    case HttpMethodTypes.Post:
                        if (resultObj == null) 
                        {
                            context.Result = new NotFoundResult();
                            break;
                        }
                        context.Result = new CreatedResult(string.Empty, null);
                        break;
                    case HttpMethodTypes.Get:
                    case HttpMethodTypes.Put:
                    case HttpMethodTypes.Delete:
                    default:
                        if (resultObj == null) 
                        {
                            context.Result = new NotFoundResult();
                            break;
                        } 
                        if (SendRawResult)
                        {
                            context.Result = (resultObj as FileResult)!;
                            break;
                        }
                        context.Result = new OkObjectResult(resultObj);
                        break;

                }
            }
        }
    }
}
