using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using MotoShop.WebAPI.Logging.Logger;
using Serilog;
using System;

namespace MotoShop.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {

            Log.Logger = LoggerHelper.CreateLogger("./Logging/logs/logs.txt");

            try
            {
                Log.Information("Starting host");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
