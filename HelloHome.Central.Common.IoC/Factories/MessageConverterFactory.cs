using System.Collections.Concurrent;
using HelloHome.Central.Common.Extensions;
using HelloHome.Central.Common.Mqtt.Converters;
using HelloHome.Central.Common.Mqtt.Topic;
using HelloHome.Central.Domain.Messages;
using Lamar;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HelloHome.Central.Common.IoC.Factories;

public class MessageConverterFactory : IMessageParserFactory, IMessageEncoderFactory
{
    private readonly IContainer _container;
    private readonly ILogger<MessageConverterFactory> _logger;
    private readonly ConcurrentDictionary<Report, Type> _parserMap;
    private readonly ConcurrentDictionary<Type, Type> _encoderMap;

    public MessageConverterFactory(IContainer container, ILogger<MessageConverterFactory> logger)
    {
        _container = container;
        _logger = logger;
        var parserTypes = container.Model.AllInstances
            .Where(_ => _.ServiceType == typeof(IMessageParser))
            .Where(_ => _.ImplementationType.HasAttribute<MapTopicAttribute>())
            .Select(_ => _.ImplementationType)
            .Distinct();
        _parserMap = new ConcurrentDictionary<Report, Type>(
            parserTypes.Select(
                t => new KeyValuePair<Report, Type>(
                    t.GetAttribute<MapTopicAttribute>()!.Report??throw new Exception($"No Report found in MapTopicAttribute on type {t.Name}"), 
                    t)));
        var encoderTypes = container.Model.AllInstances
            .Where(_ => _.ServiceType == typeof(IMessageEncoder))
            .Where(_ => _.ImplementationType.HasAttribute<MapTopicAttribute>())
            .Select(_ => _.ImplementationType)
            .Distinct();
        _encoderMap = new ConcurrentDictionary<Type, Type>(encoderTypes.Select(t => new KeyValuePair<Type, Type>(t.BaseType!.GenericTypeArguments[0], t)));
    }

    public IMessageParser GetParserFor(HhTopic topic)
    {
        var parserType = _parserMap[topic.Report??throw new Exception("No report found in topic")];
        _logger.LogTrace($"Parser for {parserType.Name} found for topic {topic}");
        return (IMessageParser)_container.GetRequiredService(parserType);
    }

    public IMessageEncoder GetEncoderFor(Message message)
    {
        var encoderType = _encoderMap[message.GetType()];
        return (IMessageEncoder)_container.GetRequiredService(encoderType);
    }
}