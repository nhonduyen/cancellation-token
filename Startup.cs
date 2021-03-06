﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using cancel.Models;

namespace cancel
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
 
            services.AddMvc(options =>
            {
                options.Filters.Add<OperationCancelledExceptionFilter>();
                options.CacheProfiles.Add("Default",
                new CacheProfile()
                {
                    Duration = 30,
                    VaryByHeader = "User-Agent",
                    Location = ResponseCacheLocation.Client
                });
                options.CacheProfiles.Add("Never",
                new CacheProfile()
                {
                    NoStore = true,
                    Location = ResponseCacheLocation.None
                });
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMvc();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ITransisionService, SomeService>();
            services.AddScoped<IScopedService, SomeService>();
            services.AddSingleton<ISingletonService, SomeService>();
            services.AddMemoryCache();
            // add hosted service 
            services.AddHostedService<TimedHostedService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // middleware for server socket events
            // this works on its own without the need for MVC.
            /* app.Use(async (context, next) =>
            {
                if (context.Request.Path.ToString().Equals("/sse1"))
                {
                    var response = context.Response;
                    response.Headers.Add("Content-Type", "text/event-stream");

                    for (var i = 0; true; ++i)
                    {
                        await response
                            .WriteAsync($"data: Middleware {i} at {DateTime.Now}\r\r");

                        response.Body.Flush();

                        await Task.Delay(5 * 1000);
                    }
                }

                await next.Invoke();
            });
            */
        }
    }
}
