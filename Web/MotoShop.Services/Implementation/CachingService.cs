using Microsoft.Extensions.Caching.Distributed;
using MotoShop.Data.Models.User;
using MotoShop.Services.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MotoShop.Services.Implementation
{

    public class CachingService : ICachingService
    {
        private readonly IDistributedCache _distributedCache;

        public CachingService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task CacheIdentityResponseAsync(string userID, object obj, TimeSpan timeToLive)
        {
            await this.CacheResponseAsync(userID, obj, timeToLive);
        }

        public async Task CacheResponseAsync(string key, object obj, TimeSpan timeToLive)
        {
            if (obj is null)
                return;

            var serializedResponse = JsonConvert.SerializeObject(obj);

            await _distributedCache.SetStringAsync(key, serializedResponse,new DistributedCacheEntryOptions 
            {
                AbsoluteExpirationRelativeToNow = timeToLive
            });
        }

        public async Task ClearCache(IEnumerable<string> keys)
        {
            foreach (var key in keys)
            {
                await _distributedCache.RemoveAsync(key);
            }
        }

        public async Task<string> GetCachedResponseAsync(string key)
        {
            var cachedResponse = await _distributedCache.GetStringAsync(key);

            return (string.IsNullOrEmpty(cachedResponse)) ? null : cachedResponse;

        }

        public async Task<string> GetIdentityCachedResponseAsync(string userID)
        {
            var response  = await this.GetCachedResponseAsync(userID);

            if(response != null)
                return response;

            return null;
        }
    }
}
