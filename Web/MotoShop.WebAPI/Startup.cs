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
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Http;
using MotoShop.Services.HelperModels;

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
            services.AddCaching(Configuration);
            services.AddCompression();
            services.AddControllers().AddJsonOptions(JsonConfiguration.Configure);
            services.AddDbContext<ApplicationDatabaseContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Production"), SqlServerConfigurations.Configure), ServiceLifetime.Transient);
            services.AddIdentity<ApplicationUser, IdentityRole>(ApplicationUserConfigurations.Configure)
                .AddEntityFrameworkStores<ApplicationDatabaseContext>()
                .AddDefaultTokenProviders();

            services.AddAuthenticationExtension(Configuration);
            services.AddAutoMapper(typeof(Startup))
                .SetUpAutoMapper();


            services.Configure<GoogleAuthOptions>(Configuration.GetSection("GoogleAuthentication"));

            services.AddCors(setup =>
            {
                setup.AddPolicy("DevPolicy", configure =>
                {
                    configure.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .SetIsOriginAllowed((host) => true);
                });
            });

            services.Configure<FormOptions>(options => 
            {
                options.MemoryBufferThreshold = int.MaxValue;
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = int.MaxValue;
            });
            services.AddApplicationServices();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("DevPolicy");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),@"wwwroot", @"resources")),
                RequestPath = new PathString("/wwwroot/resources")
            });
         
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
