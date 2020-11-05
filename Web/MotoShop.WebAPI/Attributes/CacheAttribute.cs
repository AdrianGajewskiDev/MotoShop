using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using MotoShop.Services.Services;
using MotoShop.WebAPI.Attributes.Base;
using MotoShop.WebAPI.Configurations;
using MotoShop.WebAPI.Helpers;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoShop.WebAPI.Attributes
{
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLive;

        public CacheAttribute(int timeToLive)
        {
            _timeToLive = timeToLive;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cache = context.HttpContext.RequestServices.GetRequiredService<CacheBase>();
            await cache.Cache(context, next, _timeToLive);

            //var service = context.HttpContext.RequestServices.GetRequiredService<ICachingService>();
            //var redisOptions = context.HttpContext.RequestServices.GetRequiredService<RedisOptions>();

            //if (service == null ||redisOptions.Enabled == false)
            //    await next();

            //string key = GenerateCacheKey(context.HttpContext.Request);

            //if (!CacheKeys.Keys.Contains(key))
            //    CacheKeys.Keys.Add(key);

            //var cachedResponse = await service.GetCachedResponseAsync(key);

            //if(!string.IsNullOrEmpty(cachedResponse))
            //{
            //    var cntResult = new ContentResult
            //    {
            //        Content = cachedResponse,
            //        ContentType = "application/json",
            //        StatusCode = 200
            //    };

            //    context.Result = cntResult;
            //    return;
            //}

            //var executedResult = await next();
            //if(executedResult.Result is OkObjectResult result)
            //{
            //    await service.CacheResponseAsync(key, result.Value, TimeSpan.FromMinutes(_timeToLive));
            //}
        }

    }
}
