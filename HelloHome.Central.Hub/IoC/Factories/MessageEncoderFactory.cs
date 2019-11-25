using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using HelloHome.Central.Hub.MessageChannel.Messages;
using HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Encoders;
using Lamar;

namespace HelloHome.Central.Hub.IoC.Factories
{
    public interface IMessageEncoderFactory
    {
        IMessageEncoder Build(OutgoingMessage message);
    }

    public class MessageEncoderFactory : IMessageEncoderFactory
    {
        private readonly Container _container;
        private readonly ConcurrentDictionary<Type, Type> _typeMap;

        public MessageEncoderFactory(Container container)
        {
            _container = container;
            var encoderTypes = container.Model.AllInstances
                .Where(_ => _.ServiceType == typeof(IMessageEncoder))
                .Select(_ => _.ImplementationType)
                .Distinct();

            _typeMap = new ConcurrentDictionary<Type, Type>(encoderTypes.Select(t => new KeyValuePair<Type, Type>(t.BaseType.GenericTypeArguments[0], t)));
        }
        
        public IMessageEncoder Build(OutgoingMessage message)
        {
            var msgType = message.GetType();
            if (_typeMap.TryGetValue(msgType, out var encoderType))
                return (IMessageEncoder) _container.GetInstance(encoderType);
            throw new Exception($"Encoder not found for {msgType.Name}");
        }
        
    }

}