using HelloHome.Central.Common.IoC.Registries;
using HelloHome.Central.Core.IoC;
using HelloHome.Central.Repository;
using Lamar.Microsoft.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace HelloHome.Central.Core;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args)
            .UseLamar()
            .ConfigureContainer<Lamar.ServiceRegistry>((hostContext, registry) =>
            {
                registry.IncludeRegistry(new MqttRegistry(hostContext.Configuration));
                registry.IncludeRegistry(new DbContextRegistry());
                registry.IncludeRegistry(new CommandAndQueriesRegistry());
                registry.IncludeRegistry(new BusinessLogicRegistry());
                registry.IncludeRegistry(new HandlerRegistry());
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<MqttHostedService>();
                services.AddHostedService<MqttMessageHandlerWorker>();
                services.AddDbContext<HhDbContext>(builder =>
                {
                    builder.UseMySql(hostContext.Configuration.GetConnectionString("local"), 
                        new MariaDbServerVersion(new Version(12, 4, 2)),
                        optionBuilder =>
                        {
                            //optionBuilder.ServerVersion(new Version(10, 4, 11), ServerType.MariaDb);
                        });
                    //builder.UseLoggerFactory(new NLogLoggerFactory(new NLogLoggerProvider()));
                });
            });

        var host = builder.Build();
        await host.RunAsync();
    }
}