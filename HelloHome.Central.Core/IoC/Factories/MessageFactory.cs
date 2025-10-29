using System.Collections.Concurrent;
using HelloHome.Central.Core.Mqtt.Converters;
using HelloHome.Central.Domain.Messages;
using JasperFx.Core.Reflection;
using Lamar;
using MQTTnet;

namespace HelloHome.Central.Core.IoC.Factories;

public interface IMessageFactory
{
    IncomingMessage FromMqtt(MqttApplicationMessage mqttMessage);
    MqttApplicationMessage FromMessage(OutgoingMessage message);
}

public class MessageFactory : IMessageFactory
{
    private readonly IContainer _container;
    private readonly ConcurrentDictionary<string, Type> _parserMap;
    private readonly ConcurrentDictionary<Type, Type> _encoderMap;

    public MessageFactory(IContainer container)
    {
        _container = container;
        var parserTypes = container.Model.AllInstances
            .Where(_ => _.ServiceType == typeof(IParser))
            .Where(_ => _.ImplementationType.HasAttribute<MapTopicAttribute>())
            .Select(_ => _.ImplementationType)
            .Distinct();
        _parserMap = new ConcurrentDictionary<string, Type>(parserTypes.Select(t => new KeyValuePair<string, Type>(t.GetAttribute<MapTopicAttribute>()!.Topic, t)));
        var encoderTypes = container.Model.AllInstances
            .Where(_ => _.ServiceType == typeof(IEncoder))
            .Where(_ => _.ImplementationType.HasAttribute<MapTopicAttribute>())
            .Select(_ => _.ImplementationType)
            .Distinct();
        _encoderMap = new ConcurrentDictionary<Type, Type>(encoderTypes.Select(t => new KeyValuePair<Type, Type>(t.BaseType!.GenericTypeArguments[0], t)));
    }
    public IncomingMessage FromMqtt(MqttApplicationMessage mqttMessage) 
    {
        var rptName = mqttMessage.Topic.Split('/').Last();
        var parserType = _parserMap[rptName];
        var parser = (IParser)_container.GetRequiredService(parserType);
        var inMsg =parser.Parse(mqttMessage);
        return inMsg;
    }

    public MqttApplicationMessage FromMessage(OutgoingMessage message)
    {
        var encoderType = _encoderMap[message.GetType()];
        var encoder = (IEncoder)_container.GetRequiredService(encoderType);
        return encoder.Encode(message);   
    }
}