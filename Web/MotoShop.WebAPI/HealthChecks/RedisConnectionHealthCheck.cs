using Microsoft.Extensions.Diagnostics.HealthChecks;
using MotoShop.Services.Services;
using MotoShop.WebAPI.Configurations;
using System.Threading;
using System.Threading.Tasks;

namespace MotoShop.WebAPI.HealthChecks
{
    public class RedisConnectionHealthCheck : IHealthCheck
    {
        private readonly ICachingService _cacheService;
        private readonly RedisOptions _redisOptions;

        public RedisConnectionHealthCheck(ICachingService cacheService, RedisOptions redisOptions)
        {
            _cacheService = cacheService;
            _redisOptions = redisOptions;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var result = _cacheService.Connected(_redisOptions.Host, _redisOptions.Port);

            if(result.Connected)
                return Task.FromResult(HealthCheckResult.Healthy(result.Description));
            else
                return Task.FromResult(HealthCheckResult.Unhealthy(result.Description));
        }
    }
}
