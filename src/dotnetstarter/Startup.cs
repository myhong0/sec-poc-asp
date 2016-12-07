using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Demo
{
    public class Startup
    {
        public static string ConnectionString { get; private set; }
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration     = builder.Build();
            ConnectionString = Configuration.GetConnectionString("DemoConnection");
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // inject the configuration directly
            services.AddApplicationInsightsTelemetry(Configuration);
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
               .AddCommandLine(args)
               .Build();

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseConfiguration(config)
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
