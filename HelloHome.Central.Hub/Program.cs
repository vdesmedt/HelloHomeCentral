﻿using HelloHome.Central.Domain;
using System;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Net.Mime;
using System.Threading.Tasks;
using HelloHome.Central.Common;
using HelloHome.Central.Common.Configuration;
using HelloHome.Central.Hub.IoC.Installers;
using HelloHome.Central.Hub.MessageChannel;
using HelloHome.Central.Hub.WebApi;
using HelloHome.Central.Repository;
using Lamar;
using Lamar.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace HelloHome.Central.Hub
{
    public static class Program
    {
        private static readonly Logger Logger = NLog.LogManager.GetLogger(nameof(Program));		

        public static async Task Main(string[] args)
        {
            Logger.Debug($"Starting on MachineName {Environment.MachineName}");
            var host = new HostBuilder()
                .UseSystemd()
                .UseLamar((context, registry) =>
                {
                    registry.IncludeRegistry<HubServiceRegistry>();
                })
                .ConfigureLogging((hostContext, configLogging) =>
                {
                    configLogging.ClearProviders();
                    configLogging.SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog()
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddCommandLine(args);
                })
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.SetBasePath(Directory.GetCurrentDirectory());
                    configApp.AddJsonFile($"appsettings.json", false,  true);
                    configApp.AddJsonFile($"appsettings.{Environment.MachineName}.json", true,  true);
                    configApp.AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<NodeBridge.NodeBridgeApp>();
                    services.AddDbContext<HhDbContext>(builder =>
                    {
                        builder.UseMySql(hostContext.Configuration.GetConnectionString("local"), 
                            new MariaDbServerVersion(new Version(10, 4, 11)),
                            optionBuilder =>
                        {
                            //optionBuilder.ServerVersion(new Version(10, 4, 11), ServerType.MariaDb);
                        });
                        builder.UseLoggerFactory(new NLogLoggerFactory(new NLogLoggerProvider()));
                    });
                    services.Configure<SerialConfig>(hostContext.Configuration.GetSection("Serial"));
                    services.Configure<RFM2PiConfig>(hostContext.Configuration.GetSection("RFM2Pi"));
                    services.Configure<EmonCmsConfig>(hostContext.Configuration.GetSection("EmonCms"));
                })
                .ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>())
                .Build();

            await host.RunAsync();
        }
    }
}