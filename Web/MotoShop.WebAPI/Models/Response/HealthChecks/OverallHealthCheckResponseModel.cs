using System.Collections.Generic;

namespace MotoShop.WebAPI.Models.Response.HealthChecks
{
    public class OverallHealthCheckResponseModel
    { 
        public string Status { get; set; }
        public IEnumerable<HealthCheckResultResponseModel> Entries { get; set; }
    }
}
