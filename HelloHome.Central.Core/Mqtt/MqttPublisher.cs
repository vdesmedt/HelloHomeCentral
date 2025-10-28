using MQTTnet;
using MQTTnet.Protocol;

namespace HelloHome.Central.Core.Mqtt;

public interface IMqttPublisher
{
    Task PublishAsync(string topic, string payload, CancellationToken cancellationToken);
}

public class MqttPublisher(IMqttClient mqttClient) : IMqttPublisher
{
    //TODO : Add QoS
    public async Task PublishAsync(string topic, string payload, CancellationToken cancellationToken)
    {
        var msg = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(payload)
            .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
            .WithRetainFlag(false)
            .Build();
        //TODO: Manage what to do when diconnected. Probably keep the msg in a queue and send it when connected (maybe throttling ?)
        if (mqttClient.IsConnected)
            await mqttClient.PublishAsync(msg, cancellationToken);
    }
}