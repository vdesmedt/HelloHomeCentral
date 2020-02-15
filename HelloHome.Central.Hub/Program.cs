using HelloHome.Central.Domain;
using System;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Net.Mime;
using System.Threading.Tasks;
using HelloHome.Central.Common;
using HelloHome.Central.Hub.IoC.Installers;
using HelloHome.Central.Hub.MessageChannel;
using HelloHome.Central.Hub.WebApi;
using Lamar;
using Lamar.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace HelloHome.Central.Hub
{
    public static class Program
    {
        private static readonly Logger Logger = NLog.LogManager.GetLogger(nameof(Program));		

        public static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .UseLamar((context, registry) =>
                {
                    registry.IncludeRegistry<HubServiceRegistry>();
                })
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddCommandLine(args);
                })
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.SetBasePath(Directory.GetCurrentDirectory());
                    configApp.AddJsonFile(
                        $"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json",
                        optional: true);
                    configApp.AddCommandLine(args);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging(builder => builder.SetMinimumLevel(LogLevel.Debug));
                    services.AddHostedService<NodeBridge.NodeBridgeApp>();
                })
                .ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>())
                .ConfigureLogging((hostContext, configLogging) => { configLogging.AddConsole(); })
                .UseConsoleLifetime()
                .Build();

            await host.RunAsync();
                                 
        }
    }
}