using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using MotoShop.Services.Services;
using MotoShop.WebAPI.Configurations;
using MotoShop.WebAPI.Helpers;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoShop.WebAPI.Attributes.Base
{
    public class CacheBase
    {
        public async Task Cache(ActionExecutingContext context, ActionExecutionDelegate next, int _timeToLive, bool identityCache = false)
        {
            var service = context.HttpContext.RequestServices.GetRequiredService<ICachingService>();
            var redisOptions = context.HttpContext.RequestServices.GetRequiredService<RedisOptions>();
            string userId = string.Empty;

            if (identityCache == true)
            {
                var user = context.HttpContext.User;
                if(user != null)
                    userId = user.FindFirst(x => x.Type == "UserID").Value;
            }

            if (service == null || redisOptions.Enabled == false || (userId == null && identityCache == true))
                await next();

            string key = (identityCache == true)? userId : GenerateCacheKey(context.HttpContext.Request);

            if (!CacheKeys.Keys.Contains(key))
                CacheKeys.Keys.Add(key);

            var cachedResponse = await service.GetCachedResponseAsync(key);

            if (!string.IsNullOrEmpty(cachedResponse))
            {
                var cntResult = new ContentResult
                {
                    Content = cachedResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };

                context.Result = cntResult;
                return;
            }

            var executedResult = await next();
            if (executedResult.Result is OkObjectResult result)
            {
                await service.CacheResponseAsync(key, result.Value, TimeSpan.FromMinutes(_timeToLive));
            }
        }

        private string GenerateCacheKey(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();

            keyBuilder.Append($"{request.Path}");

            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }

            return keyBuilder.ToString();
        }
    }
}
