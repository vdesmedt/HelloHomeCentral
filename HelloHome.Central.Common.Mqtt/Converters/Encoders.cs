using System.Text.Json;
using HelloHome.Central.Common.Extensions;
using HelloHome.Central.Domain.Messages;
using HelloHome.Central.Domain.Messages.Commands;
using MQTTnet;

namespace HelloHome.Central.Common.Mqtt.Converters;

public interface IEncoder 
{
    MqttApplicationMessage Encode(OutgoingMessage message);
}

public abstract class Encoder<T> : IEncoder where T : OutgoingMessage
{
    public MqttApplicationMessage Encode(OutgoingMessage message)
    {
        var topic = GetType().GetAttribute<MapTopicAttribute>()?.Topic ?? throw new Exception("Topic not found");
        return new MqttApplicationMessageBuilder()
            .WithTopic($"Node/{message.ToRfAddress}/{topic}")
            .WithPayload(JsonSerializer.Serialize((T)message))
            .Build();
    }
}

[MapTopic("config")]
public class NodeConfigEncoder : Encoder<NodeConfigCommand>;