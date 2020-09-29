using Serilog;
using Serilog.Formatting.Json;

namespace MotoShop.WebAPI.Logging.Logger
{
    public static class LoggerHelper
    {
        public static ILogger CreateLogger(string filePath)
        {
             return new LoggerConfiguration()
            .MinimumLevel.Information()
            .Enrich.FromLogContext()
            .WriteTo.File(new JsonFormatter(), filePath)
            .CreateLogger();
        }
    }
}
