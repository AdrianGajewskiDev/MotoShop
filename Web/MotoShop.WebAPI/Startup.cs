using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MotoShop.Data.Database_Context;
using MotoShop.Data.Models.User;
using MotoShop.WebAPI.Configurations;
using MotoShop.WebAPI.Extensions;
using MotoShop.WebAPI.Midleware;
using AutoMapper;
using Microsoft.AspNetCore.ResponseCompression;
using System.Collections.Generic;

namespace MotoShop.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCaching(Configuration.GetConnectionString("Redis"));
            services.AddCompression();
            services.AddControllers().AddJsonOptions(JsonConfiguration.Configure);
            services.AddDbContext<ApplicationDatabaseContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Production"), SqlServerConfigurations.Configure));
            services.AddIdentityCore<ApplicationUser>(ApplicationUserConfigurations.Configure)
                .AddEntityFrameworkStores<ApplicationDatabaseContext>()
                .AddDefaultTokenProviders();

            services.AddJwtAuthentication(Configuration["JWT:Key"]);
            services.AddAutoMapper(typeof(Startup))
                .SetUpAutoMapper();

            services.AddApplicationServices();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseResponseCompression();

            app.UseExceptionHandler(new ExceptionHandlerOptions
            {
                ExceptionHandler = new ExceptionHandlerMidleware().HandleException
            });
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(name: "default", pattern: "api/{controller}/{action?}/{params?}");
            });
        }
    }
}
