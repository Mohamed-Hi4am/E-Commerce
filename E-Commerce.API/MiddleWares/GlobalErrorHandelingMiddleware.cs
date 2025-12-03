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
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Something Went Wrong");

                await HandelExceptionAsync(httpContext, exception);
            }
        }

        private async Task HandelExceptionAsync(HttpContext httpContext, Exception
        exception)
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var problem = new ProblemDetails()
            {
                Title = "Internal Server Error",
                Detail = exception.Message,
                Status = StatusCodes.Status500InternalServerError,
                Instance = httpContext.Request.Path
            };

            await httpContext.Response.WriteAsJsonAsync(problem);
        }
    }
}
