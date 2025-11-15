using System.Text.Json;
using HelloHome.Central.Common.Extensions;
using HelloHome.Central.Common.Mqtt.Topic;
using HelloHome.Central.Domain.Messages;
using HelloHome.Central.Domain.Messages.Commands;
using MQTTnet;

namespace HelloHome.Central.Common.Mqtt.Converters;

public interface IMessageEncoderFactory
{
    IMessageEncoder GetEncoderFor(Message message);
}

public interface IMessageEncoder 
{
    MqttApplicationMessage EncodeMessage(Message message);
}

public abstract class MessageEncoder<T> : IMessageEncoder where T : Message
{
    public MqttApplicationMessage EncodeMessage(Message message)
    {
        return EncodeMessage((T)message);
    }

    protected MqttApplicationMessage EncodeMessage(T message)
    {
        return new MqttApplicationMessageBuilder()
            .WithTopic(GetTopic(message))
            .WithPayload(JsonSerializer.Serialize((T)message))
            .Build();
    }

    protected abstract string GetTopic(T message);
}

public abstract class OutgoingMessageEncoder<T> : MessageEncoder<T> where T : OutgoingMessage
{
    protected override string GetTopic(T message)
    {
        var command = GetType().GetAttribute<MapTopicAttribute>()?.Command ?? throw new Exception("Topic not found");
        return HhTopic.ForCommand(command).ToNode(message.ToRfAddress).ToString();
    }
}

public abstract class RequestEncoder<T> : MessageEncoder<T> where T : Request
{
    protected override string GetTopic(T message)
    {
        var command = GetType().GetAttribute<MapTopicAttribute>()?.Command ?? throw new Exception("Topic not found");
        return HhTopic.ForCommand(command).ToCore().ToString();
    }
}

[MapTopic(Command.Config)]
public class NodeConfigMessageEncoder : OutgoingMessageEncoder<NodeConfigCommand>;
[MapTopic(Command.Restart)]
public class RestartCommandMessageEncoder : OutgoingMessageEncoder<RestartCommand>;
