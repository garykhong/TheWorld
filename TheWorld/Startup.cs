using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TheWorld.Services;
using Microsoft.Extensions.Configuration;

namespace TheWorld
{
    public class Startup
    {
        private IConfigurationRoot config;

        public IHostingEnvironment Env { get; private set; }

        public Startup(IHostingEnvironment env)
        {
            this.Env = env;
            var builder = new ConfigurationBuilder()
                .AddJsonFile("config.json")
                .SetBasePath(env.ContentRootPath)
                .AddEnvironmentVariables();

            config = builder.Build();
            
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(this.config);

            if(Env.IsDevelopment())
            {
                services.AddScoped<MailService, DebugMailService>();
            }
            
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseStaticFiles();
            app.UseMvc(config =>
            {
                config.MapRoute(
                    name: "Default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "App", action = "Index"}
                    );
            });
        }
    }
}
