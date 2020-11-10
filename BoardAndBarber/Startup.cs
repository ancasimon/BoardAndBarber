using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoardAndBarber.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BoardAndBarber
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // whenever anybody asks for an IConfiguration, give them this Configuration:
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddTransient<CustomerRepository>();

            //services.AddTransient<>()
            //services.AddSingleton<>()
            //services.AddScoped<>()
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            // Anca: this is how we configure our application to accept - or not accept - CORS requests - in general, our application will NOT accept them!
            // You have to allow some because the front end runs on a different origin (usually localhost:8080) 
            // but since we don't know that the front end will always run only on that origin, we need to do this:
            // we will take in the policy object that ASP.Net will use to determine if a CORS request will be allowed or not:
            // This is where you can control - maybe you want to allow only get methods instead of saying Any ...
            app.UseCors(policy => policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
