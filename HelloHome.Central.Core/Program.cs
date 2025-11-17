using HelloHome.Central.Common.IoC.Registries;
using HelloHome.Central.Common.Mqtt;
using HelloHome.Central.Repository;
using Lamar.Microsoft.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;

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
                });
            })
            .ConfigureLogging((hostBuilderContext, loggingBuilder) =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddOpenTelemetry(opt =>
                {
                    opt.SetResourceBuilder(ResourceBuilder.CreateEmpty()
                        .AddService("HelloHome.Central.Core")
                        .AddAttributes(new Dictionary<string, object>
                        {
                            {"Environment", hostBuilderContext.HostingEnvironment.EnvironmentName}
                        }));
                    opt.IncludeScopes = true;
                    opt.IncludeFormattedMessage = true;
                    
                    opt.AddConsoleExporter();
                    opt.AddOtlpExporter(o =>
                    {
                        o.Endpoint = new Uri("http://seq:80/ingest/otlp/v1/logs");
                        o.Protocol = OtlpExportProtocol.HttpProtobuf;
                        o.Headers = "X-Seq-ApiKey=E21iZem6nzzgwc3vk5wa";
                    });
                });
            });
        

        var host = builder.Build();
        await host.RunAsync();
    }
}