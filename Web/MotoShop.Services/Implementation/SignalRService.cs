﻿using MotoShop.Services.Services;
using System;
using System.Threading.Tasks;

namespace MotoShop.Services.Implementation
{
    public class SignalRService : IWebSocketProviderService
    {
        private readonly ICachingService _cachingService;

        public SignalRService(ICachingService cachingService)
        {
            _cachingService = cachingService;
        }

        public async Task AddConnectionAsync(string connectionID, string data)
        {
            await _cachingService.SaveToCacheAsync(data, connectionID);
        }

        public async Task<string> GetConnectionIDAsync(string data)
        {
            return await _cachingService.GetFromCacheAsync(data);
        }

        public async Task<bool> HasActivConnection(string userID)
        {
            var result = await _cachingService.GetFromCacheAsync(userID);

            return string.IsNullOrEmpty(result);
        }

        public void RemoveConnection(string userID)
        {
            _cachingService.ClearCache(new string[] { userID });
        }

        public async Task UpdateConnectionIDAsync(string data, string newConnectionID)
        {
            if(await HasActivConnection(data))
                RemoveConnection(data);

            await AddConnectionAsync(newConnectionID, data);
        }
    }
}
