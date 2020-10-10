using System;
using System.Threading.Tasks;

namespace MotoShop.Services.Services
{
    public interface ICachingService
    {
        Task CacheResponseAsync(string key, object obj, TimeSpan timeToLive);
        Task<string> GetCachedResponseAsync(string key);
    }
}
