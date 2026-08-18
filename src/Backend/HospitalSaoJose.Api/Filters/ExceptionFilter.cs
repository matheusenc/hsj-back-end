using HospitalSaoJose.Communication.Responses;
using HospitalSaoJose.Exception;
using HospitalSaoJose.Exception.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HospitalSaoJose.Api.Filters;

public sealed class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is HospitalSaoJoseException hospitalSaoJoseException)
        {
            context.HttpContext.Response.StatusCode = (int)hospitalSaoJoseException.GetStatusCode();
            context.Result = new ObjectResult(new ResponseErrorJson(hospitalSaoJoseException.GetErrorMessages()));
        }
        else
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Result = new ObjectResult(new ResponseErrorJson(ErrorMessages.UNKNOWN_ERROR));
        }
    }
}
