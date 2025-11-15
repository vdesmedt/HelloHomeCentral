using HelloHome.Central.Common.Mqtt.Converters;
using HelloHome.Central.Domain.Messages;
using MQTTnet;
using MQTTnet.Protocol;

namespace HelloHome.Central.Common.Mqtt;

public interface IMqttPublisher
{
    Task PublishAsync(string topic, string payload, CancellationToken cancellationToken, MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtLeastOnce);
    Task PublishAsync(string topic, Message message, CancellationToken cancellationToken, MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtLeastOnce);
}

public class MqttPublisher(IMqttClient mqttClient, IMessageEncoderFactory messageEncoderFactory) : IMqttPublisher
{
    //TODO : Add QoS
    public async Task PublishAsync(string topic, string payload, CancellationToken cancellationToken, MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtLeastOnce)
    {
        var msg = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(payload)
            .WithQualityOfServiceLevel(qos)
            .WithRetainFlag(false)
            .Build();
        //TODO: Manage what to do when diconnected. Probably keep the msg in a queue and send it when connected (maybe throttling ?)
        if (mqttClient.IsConnected)
            await mqttClient.PublishAsync(msg, cancellationToken);
    }

    public async Task PublishAsync(string topic, Message message, CancellationToken cancellationToken, MqttQualityOfServiceLevel qos)
    {
        var msgEncoder = messageEncoderFactory.GetEncoderFor(message);
        var appMsg = msgEncoder.EncodeMessage(message);
        appMsg.Topic = topic;
        appMsg.QualityOfServiceLevel = qos;
        if (mqttClient.IsConnected)
            await mqttClient.PublishAsync(appMsg, cancellationToken);
    }
}