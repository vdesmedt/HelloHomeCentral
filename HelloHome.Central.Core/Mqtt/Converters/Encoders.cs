using System.Text.Json;
using HelloHome.Central.Domain.Messages;
using HelloHome.Central.Domain.Messages.Commands;
using JasperFx.Core.Reflection;
using MQTTnet;

namespace HelloHome.Central.Core.Mqtt.Converters;

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
            .WithTopic(topic)
            .WithPayload(JsonSerializer.Serialize((T)message))
            .Build();
    }
}

[MapTopic("config")]
public class NodeConfigEncoder : Encoder<NodeConfigCommand>;