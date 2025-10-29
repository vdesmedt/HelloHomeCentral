using HelloHome.Central.Common.IoC;
using HelloHome.Central.Common.IoC.Factories;
using HelloHome.Central.Core.IoC.Factories;
using HelloHome.Central.Core.Mqtt;
using HelloHome.Central.Core.Mqtt.Converters;
using HelloHome.Central.Domain.Handlers.Base;
using Lamar;
using MQTTnet;

namespace HelloHome.Central.Core.IoC.ServiceRegistry;

public class MqttRegistry : Lamar.ServiceRegistry
{
    public MqttRegistry(IConfiguration config)
    {
        this.Configure<MqttSettings>(config.GetSection("Mqtt"));
        this.AddSingleton<MqttClientFactory>();
        this.AddTransient<IMqttPublisher, MqttPublisher>();
        this.AddSingleton<IMqttSubscriber, MqttSubscriber>();
        this.AddSingleton<IMessageFactory, MessageFactory>(sp =>
        {
            var ctn = sp.GetRequiredService<IContainer>();
            return new MessageFactory(ctn);
        });
        Scan(scanner =>
        {
            scanner.AssemblyContainingType<IParser>();
            scanner.Include(_ => _.GetInterfaces().Contains(typeof(IParser)));
            scanner.Include(_ => _.GetInterfaces().Contains(typeof(IEncoder)));
            scanner.Convention<WithAllInterfacesRegistrationConvention>();
        });
        this.AddSingleton<IMqttClient>(provider =>
        {
            var factory = provider.GetRequiredService<MqttClientFactory>();
            return factory.CreateMqttClient();
        });
    }
}