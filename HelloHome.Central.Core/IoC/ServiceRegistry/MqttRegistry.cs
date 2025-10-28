using HelloHome.Central.Core.Mqtt;
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
        this.AddSingleton<IMqttClient>(provider =>
        {
            var factory = provider.GetRequiredService<MqttClientFactory>();
            return factory.CreateMqttClient();
        });
        this.AddTransient<IMqttPublisher, MqttPublisher>();
    }
}