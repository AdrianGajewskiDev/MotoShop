using Microsoft.AspNetCore.Identity;

namespace MotoShop.WebAPI.Configurations
{
    public class ApplicationUserConfigurations
    {
        public static void Configure(IdentityOptions options)
        {
            //User options
            options.User.RequireUniqueEmail = true;

            //Password options
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = true;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 0;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = true;

        }
    }
}
