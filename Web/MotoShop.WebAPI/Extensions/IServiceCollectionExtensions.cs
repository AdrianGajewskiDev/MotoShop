using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MotoShop.Services.Implementation;
using MotoShop.Services.Services;
using MotoShop.WebAPI.Token_Providers;
using System;
using System.Text;

namespace MotoShop.WebAPI.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, string key)
        {
            var jwtKey = Encoding.UTF8.GetBytes(key);

            var tokentValidationParams = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(jwtKey),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options => 
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => 
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = tokentValidationParams;
            });

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //Scoped
            services.AddScoped<IApplicationUserService, ApplicationUserService>();


            //Singletons
            services.AddSingleton<JsonWebTokenWriter>();

            return services;
        }
    }
}
