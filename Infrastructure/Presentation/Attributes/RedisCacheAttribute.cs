using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstraction.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Attributes
{
    internal class RedisCacheAttribute(int durationInSec = 120) : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IServiceManager>().CacheService;

            string cacheKey = GenerateCacheKey(context.HttpContext.Request);

            // Search Redis DB for the request value by the request key (if exists).
            var result = await cacheService.GetCachedValueAsync(cacheKey);

            // If the data is already cached ("result" has value)
            if (result != null)
            {
                context.Result = new ContentResult
                {
                    Content = result,
                    ContentType = "Application/Json",
                    StatusCode = StatusCodes.Status200OK
                };

                return;
            }

            var newContext = await next.Invoke();

            if (newContext.Result is OkObjectResult okObject)
                await cacheService.SetCacheValueAsync(cacheKey, okObject.Value!, TimeSpan.FromSeconds(durationInSec));
        }

        private string GenerateCacheKey(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();

            // Request.Path  example: /api/Products
            // Request.Query example: sort=priceAsc&pageIndex=1&pageSize=5

            keyBuilder.Append(request.Path);

            foreach (var item in request.Query.OrderBy(q => q.Key))
            {
                keyBuilder.Append($"|{item.Key}-{item.Value}");
            }

            return keyBuilder.ToString();
        }
    }
}