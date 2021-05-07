using Microsoft.Extensions.Caching.Distributed;
using MotoShop.Data.Models.User;
using MotoShop.Services.Redis;
using MotoShop.Services.Services;
using Newtonsoft.Json;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public RedisConnectionResult Connected(string host, int port)
        {
            try
            {
                using(var client = new RedisClient(host,port))
                {
                    var response = client.Info;

                    if(response is not null && response.Any())
                    {
                        return RedisConnectionResult.ConnectionSucceeded("Connected");
                    }

                    return RedisConnectionResult.ConnectionFailed("Failed to connect with redis.");
                }
            }
            catch(Exception ex)
            {
                return RedisConnectionResult.ConnectionFailed($"Failed to connect with redis. Exception of type {ex.GetType().FullName} was thrown. Message: {ex.Message}");
            }
        }
        public async Task<string> GetCachedResponseAsync(string key)
        {
            var cachedResponse = await _distributedCache.GetStringAsync(key);

            return (string.IsNullOrEmpty(cachedResponse)) ? null : cachedResponse;
        }

        public async  Task<string> GetFromCacheAsync(string key)
        {
            return await _distributedCache.GetStringAsync(key);
        }

        public async Task SaveToCacheAsync(string key, string value)
        {
            await _distributedCache.SetStringAsync(key, value);
        }
    }
}
