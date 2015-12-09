using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;
using WebApp.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using WebApp.ViewModels;

namespace WebApp
{
    public class Startup
    {
        public static IConfigurationRoot Configuration;

        public Startup (IApplicationEnvironment appEnv)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(appEnv.ApplicationBasePath)
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddJsonOptions(opt => opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());
            services.AddLogging();
            services.AddScoped<IMailService, DebugMailService>();
            services.AddEntityFramework().AddSqlServer().AddDbContext<WebAppContext>();
            services.AddTransient<WebAppContextSeeder>();
            services.AddScoped<IWebAppRepository, WebAppRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, WebAppContextSeeder seeder, ILoggerFactory logFactory)
        {
            logFactory.AddDebug(LogLevel.Warning);

            app.UseStaticFiles();

            AutoMapper.Mapper.Initialize(config => {
                config.CreateMap<Trip, TripViewModel>().ReverseMap();
            });

            AutoMapper.Mapper.Initialize(config =>
            {
                config.CreateMap<Stop, StopViewModel>().ReverseMap();
            });

            app.UseMvc(config => {
                config.MapRoute(
                    name: "Default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "WebApp", action = "Index"}
                );
            });

            seeder.SeedDatabase();
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
