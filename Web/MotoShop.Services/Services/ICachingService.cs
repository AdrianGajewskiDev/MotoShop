using MotoShop.Data.Models.User;
using MotoShop.Services.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MotoShop.Services.Services
{
    public interface ICachingService
    {
        Task CacheResponseAsync(string key, object obj, TimeSpan timeToLive);
        Task<string> GetCachedResponseAsync(string key);
        Task ClearCache(IEnumerable<string> keys);
        RedisConnectionResult Connected(string host, int port);
        public Task SaveToCacheAsync(string key, string value);
        public Task<string> GetFromCacheAsync(string key);
    }
}
