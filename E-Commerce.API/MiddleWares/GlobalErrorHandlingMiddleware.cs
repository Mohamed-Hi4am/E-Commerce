using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.MiddleWares
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;

        public GlobalErrorHandlingMiddleware(RequestDelegate next,
            ILogger<GlobalErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);
                if (httpContext.Response.StatusCode == StatusCodes.Status404NotFound)
                    await HandleNotFoundEndPointAsync(httpContext);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Something Went Wrong");

                await HandleExceptionAsync(httpContext, exception);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            var Problem = new ProblemDetails()
            {
                Title = "Error While Processing The HTTP Request",
                Detail = exception.Message,
                Instance = httpContext.Request.Path,
                Status = exception switch
                {
                    NotFoundException => StatusCodes.Status404NotFound,
                    UnAuthorizedException => StatusCodes.Status401Unauthorized,
                    ValidationException => StatusCodes.Status400BadRequest,
                    _ => StatusCodes.Status500InternalServerError
                }
            };

            if (exception is ValidationException validationExceptioin)
            {
                Problem.Extensions.Add("Errors", validationExceptioin.Errors);
            }

            httpContext.Response.StatusCode = Problem.Status.Value;

            await httpContext.Response.WriteAsJsonAsync(Problem);
        }

        private async Task HandleNotFoundEndPointAsync(HttpContext httpContext)
        {
            var Response = new ProblemDetails()
            {
                Title = "Error While Processing The Request - EndPoint Not Found",
                Detail = $"The Endpoint {httpContext.Request.Path} is Not found",
                Status = StatusCodes.Status404NotFound,
                Instance = httpContext.Request.Path
            };

            await httpContext.Response.WriteAsJsonAsync(Response);
        }
    }
}
