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
using MotoShop.WebAPI.HealthChecks;
using MotoShop.WebAPI.FileProviders;

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

            services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDatabaseContext>()
                .AddCheck<RedisConnectionHealthCheck>("RedisConnection");

            services.AddAuthenticationExtension(Configuration);
            services.AddAutoMapper(typeof(Startup))
                .SetUpAutoMapper();


            services.Configure<GoogleAuthOptions>(Configuration.GetSection("GoogleAuthentication"));
            services.AddCors(setup =>
            {
                setup.AddPolicy("DevPolicy", configure =>
                {
                    configure.WithOrigins("http://localhost:4200")
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowCredentials();
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

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            var fileProvider = app.ApplicationServices.GetRequiredService<IFilesPathProvider>();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(fileProvider.PathToSave),
                RequestPath = new PathString(fileProvider.RequestPath)
            });
         
            app.UseResponseCompression();

            app.UseCors("DevPolicy");
            app.UseExceptionHandler(new ExceptionHandlerOptions
            {
                ExceptionHandler = new ExceptionHandlerMidleware().HandleException
            });

            app.UseMiddleware<CorsMidleware>();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(name: "default", pattern: "api/{controller}/{action?}/{params?}");
                endpoints.MapHealthChecks("/serverHealth");
            });
        }
    }
}
