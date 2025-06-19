using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Resources;
using Shared.GeneralModels;


namespace Api
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var statusCode = StatusCodes.Status500InternalServerError;

            var response = new ResponseModel<object>
            {
                Success = false,
                Message = NotifiAndAlertsResources.SomethingWentWrongAlert,
                Errors = new List<string> { context.Exception.Message },
                StatusCode = statusCode
            };

            context.Result = new ObjectResult(response)
            {
                StatusCode = statusCode
            };
            context.ExceptionHandled = true;
        }
    }

}
