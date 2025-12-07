using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.MiddleWares
{
    public class GlobalErrorHandelingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandelingMiddleware> _logger;

        public GlobalErrorHandelingMiddleware(RequestDelegate next,
            ILogger<GlobalErrorHandelingMiddleware> logger)
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
                    await HandelNotFoundEndPointAsync(httpContext);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Something Went Wrong");

                await HandelExceptionAsync(httpContext, exception);
            }
        }

        private async Task HandelExceptionAsync(HttpContext httpContext, Exception exception)
        {
            var Problem = new ProblemDetails()
            {
                Title = "Error While Processing The HTTP Request",
                Detail = exception.Message,
                Instance = httpContext.Request.Path,
                Status = exception switch
                {
                    NotFoundException => StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status500InternalServerError
                }
            };

            httpContext.Response.StatusCode = Problem.Status.Value;

            await httpContext.Response.WriteAsJsonAsync(Problem);
        }

        private async Task HandelNotFoundEndPointAsync(HttpContext httpContext)
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
