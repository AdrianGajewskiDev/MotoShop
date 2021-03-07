using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MotoShop.WebAPI.Models.Response.HealthChecks
{
    public class HealthCheckResultResponseModel
    {
        public HealthStatus Status { get; set; }

        public string StatusDescritpion 
        { 
            get 
            {
                switch (Status)
                {
                    case HealthStatus.Unhealthy: return HealthStatus.Unhealthy.ToString();
                    case HealthStatus.Degraded: return HealthStatus.Degraded.ToString();
                    case HealthStatus.Healthy: return HealthStatus.Healthy.ToString();
                }

                return "Unknown Health Status";
            }
        }

        public string HealthCheckName { get; set; }
        public string Description { get; set; }
    }
}
