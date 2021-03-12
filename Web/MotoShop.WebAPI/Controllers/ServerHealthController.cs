using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MotoShop.WebAPI.Helpers.HealthChecks;
using MotoShop.WebAPI.Models.Response.HealthChecks;
using static Serilog.Log;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MotoShop.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServerHealthController : ControllerBase
    {
        private readonly HealthCheckService _healthCheckService;

        public ServerHealthController(HealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        [HttpGet]
        public async Task<IActionResult> OverallHealth()
        {
            var report = await _healthCheckService.CheckHealthAsync();

            var responseModel = new OverallHealthCheckResponseModel
            {
                Status = report.Status.ToString(),
                Entries = report.Entries.Select(entry => new HealthCheckResultResponseModel 
                {
                    Status = entry.Value.Status,
                    HealthCheckName = entry.Key,
                    Description = entry.Value.Description
                })
            };

            return Ok(responseModel);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> DatabaseHealth()
        {
            var report = await _healthCheckService.CheckHealthAsync();

            Nullable<HealthReportEntry> databaseReport = report.Entries[HealthChecksConstants.DatabaseHealthCheck];

            if(databaseReport is null)
            {
                var response = new HealthCheckResultResponseModel
                {
                    HealthCheckName = HealthChecksConstants.DatabaseHealthCheck,
                    Status = HealthStatus.Unhealthy,
                    Description = "Cannot find a health check or something went wrong while trying to execute health check"
                };

                Logger.Error(response.Description, response.HealthCheckName);

                return NotFound(response);
            }

            var responseModel = new HealthCheckResultResponseModel
            {
                HealthCheckName = HealthChecksConstants.DatabaseHealthCheck,
                Status = databaseReport.Value.Status,
                Description = databaseReport.Value.Description
            };

            if(responseModel.Status != HealthStatus.Healthy)
                Logger.Error(responseModel.Description, responseModel.HealthCheckName);
            else
                Logger.Information(responseModel.Description, responseModel.HealthCheckName);

            return Ok(responseModel);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> RedisConnectionHealth()
        {
            var report = await _healthCheckService.CheckHealthAsync();

            Nullable<HealthReportEntry> redisReport = report.Entries[HealthChecksConstants.RedisConnectionCheck];

            if (redisReport is null)
            {
                var response = new HealthCheckResultResponseModel
                {
                    HealthCheckName = HealthChecksConstants.RedisConnectionCheck,
                    Status = HealthStatus.Unhealthy,
                    Description = "Cannot find a health check or something went wrong while trying to execute health check"
                };

                Logger.Error(response.Description, response.HealthCheckName);

                return NotFound(response);
            }

            var responseModel = new HealthCheckResultResponseModel
            {
                HealthCheckName = HealthChecksConstants.RedisConnectionCheck,
                Status = redisReport.Value.Status,
                Description = redisReport.Value.Description
            };

            if (responseModel.Status != HealthStatus.Healthy)
                Logger.Error(responseModel.Description, responseModel.HealthCheckName);
            else
                Logger.Information(responseModel.Description, responseModel.HealthCheckName);

            return Ok(responseModel);
        }

    }
}
