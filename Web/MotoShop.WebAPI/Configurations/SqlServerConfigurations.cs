using Microsoft.EntityFrameworkCore.Infrastructure;

namespace MotoShop.WebAPI.Configurations
{
    public class SqlServerConfigurations
    {
        public static void Configure(SqlServerDbContextOptionsBuilder builder)
        {
            builder.EnableRetryOnFailure(2);
            builder.CommandTimeout(120);
        }
    }
}
