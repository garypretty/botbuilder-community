﻿using Bot.Builder.Community.Adapters.ActionsSDK;
using Bot.Builder.Community.Adapters.Google;
using Bot.Builder.Community.Adapters.Google.Core.Model.Request;
using Bot.Builder.Community.Samples.Google.Bots;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bot.Builder.Community.Samples.Google
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Create the Bot Framework Adapter with error handling enabled.
            services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();

            services.AddSingleton(sp => new ActionsSdkAdapterOptions()
            {
                ActionInvocationName = "new adapter",
                ActionProjectId = "new-action-e43a1",
                ShouldEndSessionByDefault = false,
            });

            services.AddSingleton<ActionsSdkAdapter, ActionsSdkAdapterWithErrorHandler>();
            
            // Create the bot as a transient. In this case the ASP Controller is expecting an IBot.
            services.AddTransient<IBot, EchoBot>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            //app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}