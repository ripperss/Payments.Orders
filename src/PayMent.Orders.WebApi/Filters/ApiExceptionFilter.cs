using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PayMent.Orders.Domain.Exception;
using PayMent.Orders.WebApi.Models;

namespace PayMent.Orders.WebApi.Filters;

public class ApiExceptionFilter : IExceptionFilter
{
    private readonly ILogger logger;

    public ApiExceptionFilter(ILogger logger)
    {
        this.logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        var exception = context.Exception;
        int statusCode = 400;
        ApiErrorResponse? response;

        switch (true)
        {
            case { } when exception is DublicateEntityException:
                {
                    response = new ApiErrorResponse
                    {
                        Code = 10,
                        Message = exception.Message,
                        Description = exception.ToText()
                    };
                    break;
                }
            case { } when exception is EntityNotFoundException:
                {
                    statusCode = 404;
                    response = new ApiErrorResponse
                    {
                        Code = 20,
                        Message = exception.Message,
                        Description = exception.ToText()
                    };
                    break;
                }               
            default:
                {
                    response = new ApiErrorResponse
                    {
                        Code = 666,
                        Message = exception.Message,
                        Description = exception.ToText()
                    };
                    break;
                }
        }

        logger.LogError($"Api method {context.HttpContext.Request.Path} finished with code {statusCode} and error: " +
                        $"{JsonSerializer.Serialize(response)}");
        context.Result = new JsonResult(new { response }) { StatusCode = statusCode };
    }
}
