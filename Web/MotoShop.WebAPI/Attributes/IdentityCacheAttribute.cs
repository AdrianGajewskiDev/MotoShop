using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using MotoShop.Services.Services;
using MotoShop.WebAPI.Configurations;
using System;
using System.Threading.Tasks;

namespace MotoShop.WebAPI.Attributes
{
    public class IdentityCacheAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLive;

        public IdentityCacheAttribute(int timeToLive) 
        {
            _timeToLive = timeToLive;
        }

        public  async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var service = context.HttpContext.RequestServices.GetRequiredService<ICachingService>();
            var redisOptions = context.HttpContext.RequestServices.GetRequiredService<RedisOptions>();
            var user = context.HttpContext.User;

            if (service == null || redisOptions.Enabled == false || user == null)
                await next();

            string userID = user.FindFirst(x => x.Type == "UserID").Value;

            string userCachedResponse = await service.GetIdentityCachedResponseAsync(userID);

            if(userCachedResponse != null)
            {
                var cntResult = new ContentResult
                {
                    Content = userCachedResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };

                context.Result = cntResult;
                return;
            }

            var executedResult = await next();
            if(executedResult.Result is OkObjectResult result)
            {
                await service.CacheIdentityResponseAsync(userID, result.Value, TimeSpan.FromMinutes(_timeToLive));
            }

        }
    }
}
