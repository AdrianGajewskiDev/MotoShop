using MotoShop.Data.Models.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MotoShop.Services.Services
{
    public interface ICachingService
    {
        Task CacheResponseAsync(string key, object obj, TimeSpan timeToLive);
        Task<string> GetCachedResponseAsync(string key);
        Task CacheIdentityResponseAsync(string userID, object obj, TimeSpan timeToLive);
        Task<string> GetIdentityCachedResponseAsync(string userID);
        Task ClearCache(IEnumerable<string> keys);
    }
}
