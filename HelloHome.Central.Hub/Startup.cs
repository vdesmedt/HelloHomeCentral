using System;
using System.IO;
using Castle.Facilities.AspNetCore;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using HelloHome.Central.Common.Configuration;
using HelloHome.Central.Hub.IoC.Installers;
using HelloHome.Central.Hub.WebAPI.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HelloHome.Central.Hub.WebAPI
{
    public class Startup
    {
        public static readonly WindsorContainer IoCContainer = new WindsorContainer();


        public Startup(IHostingEnvironment env)
        {
            var configRoot = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile($"appsettings.json", optional:false, reloadOnChange:true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional:true, reloadOnChange:true)
                .AddJsonFile($"appsettings.{Environment.MachineName}.json", optional:true, reloadOnChange:true)
                .AddEnvironmentVariables();
            Configuration = configRoot.Build();            
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // Windsor integration od chere : https://github.com/castleproject/Windsor/blob/master/docs/aspnetcore-facility.md
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            IoCContainer.AddFacility<AspNetCoreFacility>(f => f.CrossWiresInto(services));
            
            // Add framework services.
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddOptions();
            services.Configure<SerialConfig>(Configuration.GetSection("Serial"));
            services.Configure<EmonCmsConfig>(Configuration.GetSection("EmonCms"));            
            
            // Custom application component registrations, ordering is important here
            RegisterApplicationComponents(services);

            return services.AddWindsor(IoCContainer,
                opts => opts.UseEntryAssembly(typeof(ValuesController).Assembly));

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

            app.UseHttpsRedirection();
            app.UseMvc();
        }   
        
        
        private void RegisterApplicationComponents(IServiceCollection services)
        {
            // Application components
            IoCContainer.Install(                
                new FacilityInstaller(),
                new HubInstaller(),
                new HandlerInstaller(),
                new BusinessLogicInstaller(),
                new CommandAndQueriesInstaller(),
                new MessageChannelInstaller(),
                new DbContextInstaller(Configuration.GetValue<string>("ConnectionString"))
            );
        }
              
    }
}