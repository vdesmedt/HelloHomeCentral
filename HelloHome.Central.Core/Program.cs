using HelloHome.Central.Core.IoC.ServiceRegistry;
using HelloHome.Central.Core.Mqtt;
using Lamar.Microsoft.DependencyInjection;

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
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<MqttHostedService>();
                services.AddHostedService<MqttMessageHandlerWorker>();
            });

        var host = builder.Build();
        await host.RunAsync();
    }
}