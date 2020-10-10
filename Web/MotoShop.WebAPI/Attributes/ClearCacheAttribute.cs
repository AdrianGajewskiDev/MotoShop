using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using MotoShop.Services.Services;
using MotoShop.WebAPI.Helpers;
using System;
using System.Threading.Tasks;

namespace MotoShop.WebAPI.Attributes
{
    public class ClearCacheAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var service = context.HttpContext.RequestServices.GetRequiredService<ICachingService>();

            if (service == null)
                await next();

            if(CacheKeys.Keys.Count > 0)
                await service.ClearCache(CacheKeys.Keys);

            await next();
        }
    }
}
