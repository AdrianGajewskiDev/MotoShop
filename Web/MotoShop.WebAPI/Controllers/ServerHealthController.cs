using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
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

    }
}

//database check => make call to /serverHealth and grab the DatabaseHealthCheckResult
//some service check => make call to /serverHealth and grab the SomeServiceCheckResult