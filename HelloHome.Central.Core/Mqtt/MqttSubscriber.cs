using MQTTnet;

namespace HelloHome.Central.Core.Mqtt;

public interface IMqttSubscriber
{
    Task SubscribeAsync(string topic,  CancellationToken cancellationToken, Action<MqttApplicationMessageReceivedEventArgs> handler);
}

public class MqttSubscriber : IMqttSubscriber
{
    private readonly Dictionary<string, Action<MqttApplicationMessageReceivedEventArgs>> _register = new();
    private readonly MqttClientFactory _mqttClientFactory;
    private readonly IMqttClient _mqttClient;

    public MqttSubscriber(MqttClientFactory mqttClientFactory, 
        IMqttClient mqttClient)
    {
        _mqttClientFactory = mqttClientFactory;
        _mqttClient = mqttClient;
        _mqttClient.ApplicationMessageReceivedAsync += OnMqttClientOnApplicationMessageReceivedAsync;
    }

    private Task OnMqttClientOnApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs e)
    {
        if (_register.TryGetValue(e.ApplicationMessage.Topic, out var handler))
        {
            handler(e);
        }

        return Task.CompletedTask;
    }

    public async Task SubscribeAsync(string topic,  CancellationToken cancellationToken, Action<MqttApplicationMessageReceivedEventArgs> handler)
    {
        _register.Add(topic, handler);
        await _mqttClient.SubscribeAsync(
            _mqttClientFactory.CreateTopicFilterBuilder()
                .WithTopic(topic)
                .Build()
            , cancellationToken);       
    }
}