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
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Mvc;

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
            //To use MVC, add the AddMvc() service. The CamelCaseProperty... changes any Json object properties from what ever case
            //it is to camel case.
            services.AddMvc(config => 
            {
#if !DEBUG
                config.Filters.Add(new RequireHttpsAttribute());
#endif
            })
            .AddJsonOptions(opt => opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());

            //This is added to configure the users and roles and the context in where to find the WebAppUser
            services.AddIdentity<WebAppUser, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
                config.Password.RequiredLength = 8;
                config.Password.RequireUppercase = false;
                config.Password.RequireNonLetterOrDigit = false;
                config.Cookies.ApplicationCookie.LoginPath = "/Auth/Login";
            })
            .AddEntityFrameworkStores<WebAppContext>();

            //Added to allow the use of ILogger.
            services.AddLogging();

            //Add the Mail service. This is where you setup a service to test it. In this example, DebugMailService is being used to
            //test. Once tested DebugMailService can be changed to the concrete implementation class.
            services.AddScoped<IMailService, DebugMailService>();

            services.AddScoped<GeoCodingService>();

            //Add the EntityFramework and Sql Server functionality to application and supply the context to be used for the applciation.
            services.AddEntityFramework().AddSqlServer().AddDbContext<WebAppContext>();

            //This is added as transient since it only needs to be called once when the app starts to make sure the db has been seeded.
            services.AddTransient<WebAppContextSeeder>();

            //Similar to IMailService
            services.AddScoped<IWebAppRepository, WebAppRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // The order matters in this method.
        public async void Configure(IApplicationBuilder app, WebAppContextSeeder seeder, ILoggerFactory logFactory)
        {
            //Log any Warnings
            logFactory.AddDebug(LogLevel.Warning);

            app.UseStaticFiles();

            app.UseIdentity(); //Add the Identity service to the application

            AutoMapper.Mapper.Initialize(config => {
                config.CreateMap<Trip, TripViewModel>().ReverseMap();
                config.CreateMap<Stop, StopViewModel>().ReverseMap();
            });

            app.UseMvc(config => {
                config.MapRoute(
                    name: "Default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "WebApp", action = "Index"}
                );
            });

            await seeder.SeedDatabaseAsync();
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
