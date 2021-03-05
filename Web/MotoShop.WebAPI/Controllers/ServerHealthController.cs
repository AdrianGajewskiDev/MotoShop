using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MotoShop.WebAPI.Helpers.HealthChecks;
using MotoShop.WebAPI.Models.Response.HealthChecks;
using System;
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

            return Ok(report);
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

                return NotFound(response);
            }

            var responseModel = new HealthCheckResultResponseModel
            {
                HealthCheckName = HealthChecksConstants.DatabaseHealthCheck,
                Status = databaseReport.Value.Status,
                Description = databaseReport.Value.Description
            };

            return Ok(responseModel);
        }

    }
}
