using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers;
using HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers.Base;
using JasperFx.Core.Reflection;
using Lamar;

namespace HelloHome.Central.Hub.IoC.Factories
{
    public interface IMessageParserFactory
    {
        IMessageParser Build(byte[] rawBytes);
    }

    public class MessageParserFactory : IMessageParserFactory
    {
        private readonly Container _container;
        private readonly ConcurrentDictionary<byte, Type> _typeMap;

        public MessageParserFactory(Container container)
        {
            _container = container;
            var handlerTypes = container.Model.AllInstances
                .Where(_ => _.ServiceType == typeof(IMessageParser) && _.ImplementationType.HasAttribute<ParserForAttribute>() && ! _.ImplementationType.HasAttribute<NonDiscriminatedParserAttribute>())
                .Select(_ => _.ImplementationType)
       @         .Distinct();

            byte discriminatorOf(Type parser) => parser.GetCustomAttribute<ParserForAttribute>().RawDiscriminator;

            _typeMap = new ConcurrentDictionary<byte, Type>(handlerTypes.Select(t => new KeyValuePair<byte, Type>(discriminatorOf(t), t)));
        }
        
        public IMessageParser Build(byte[] rawBytes)
        {
            if (rawBytes.Length >= 2 && rawBytes[0] == '/' && rawBytes[1] == '/')
                return _container.GetInstance<CommentParser>();

            var type = _typeMap.TryGetValue(rawBytes[4], out var parserType) ? parserType : typeof(ParseAllParser);
            return (IMessageParser)_container.GetInstance(type);
        }
        
    }
}