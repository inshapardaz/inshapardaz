﻿using Inshapardaz.Api.Configuration;
using Inshapardaz.Api.Configuration.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Inshapardaz.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            Configuration = env.InitialiseConfiguration();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.RegisterFramework(Configuration)
                .ConfigureDomain(Configuration)
                .ConfigureCommandProcessor()
                .ConfigureDarker()
                .RegisterRenderes()
                .RegisterSwagger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.ConfigureLogging(Configuration);
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseWelcomePage("/welcome");
            app.ConfigureApiAuthentication(Configuration)
                .ConfigureApplication()
                .ConfigureObjectMappings()
                .ConfigureSwagger()
                .ConfigureHangfire();
        }
    }
}