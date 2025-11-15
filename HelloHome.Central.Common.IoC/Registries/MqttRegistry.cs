using HelloHome.Central.Common.IoC.Factories;
using HelloHome.Central.Common.Mqtt;
using HelloHome.Central.Common.Mqtt.Converters;
using Lamar;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet;

namespace HelloHome.Central.Common.IoC.Registries;

public class MqttRegistry : Lamar.ServiceRegistry
{
    public MqttRegistry(IConfiguration config)
    {
        this.Configure<MqttSettings>(config.GetSection("Mqtt"));
        this.AddSingleton<MqttClientFactory>();
        this.AddTransient<IMqttPublisher, MqttPublisher>();
        this.AddSingleton<IMqttSubscriber, MqttSubscriber>();
        this.AddSingleton<MessageConverterFactory>();
        For<IMessageParserFactory>().Use(ctx => ctx.GetInstance<MessageConverterFactory>());
        For<IMessageEncoderFactory>().Use(ctx => ctx.GetInstance<MessageConverterFactory>());
        
        Scan(scanner =>
        {
            scanner.AssemblyContainingType<IMessageParser>();
            scanner.Include(_ => _.GetInterfaces().Contains(typeof(IMessageParser)));
            scanner.Include(_ => _.GetInterfaces().Contains(typeof(IMessageEncoder)));
            scanner.Convention<WithAllInterfacesRegistrationConvention>();
        });
        this.AddSingleton<IMqttClient>(provider =>
        {
            var factory = provider.GetRequiredService<MqttClientFactory>();
            return factory.CreateMqttClient();
        });
    }
}