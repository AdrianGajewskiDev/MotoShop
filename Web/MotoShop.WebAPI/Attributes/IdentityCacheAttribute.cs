using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using MotoShop.WebAPI.Attributes.Base;
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
            var cache = context.HttpContext.RequestServices.GetRequiredService<CacheBase>();
            await cache.Cache(context, next, _timeToLive, true);
        }
    }
}
