using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using HelloHome.Central.Hub.Handlers.Base;
using HelloHome.Central.Hub.MessageChannel.Messages;
using Lamar;

namespace HelloHome.Central.Hub.IoC.Factories
{
    public interface IMessageHandlerFactory
    {
        IMessageHandler Build(IncomingMessage message);
        MessageHandlerScope BuildInNestedScope(IncomingMessage message);
    }

    public class MessageHandlerScope : IDisposable
    {
        private readonly INestedContainer _nestedNestedContainer;
        public readonly IMessageHandler Handler;

        public MessageHandlerScope(INestedContainer nestedContainer, Type handlerType)
        {
            _nestedNestedContainer = nestedContainer;
            Handler = (IMessageHandler)nestedContainer.GetInstance(handlerType);
        }
        
        public void Dispose()
        {
            _nestedNestedContainer.Dispose();
        }
    }

    public class MessageHandlerFactory : IMessageHandlerFactory
    {
        private readonly Container _container;
        private readonly ConcurrentDictionary<Type, Type> _typeMap;

        public MessageHandlerFactory(Container container)
        {
            _container = container;
            var handlerTypes = container.Model.AllInstances
                .Where(_ => _.ServiceType == typeof(IMessageHandler))
                .Select(_ => _.ImplementationType)
                .Distinct();
            
            _typeMap = new ConcurrentDictionary<Type, Type>(handlerTypes.Select(t =>
                new KeyValuePair<Type, Type>(t.BaseType.GenericTypeArguments[0], t)));
        }

        public IMessageHandler Build(IncomingMessage message)
        {
            var reqType = message.GetType();
            if (_typeMap.TryGetValue(reqType, out var handlerType))
                return (IMessageHandler) _container.GetInstance(handlerType);
            throw new Exception($"Handler not found for {reqType.Name}");
        }

        public MessageHandlerScope BuildInNestedScope(IncomingMessage message)
        {
            var reqType = message.GetType();
            if (_typeMap.TryGetValue(reqType, out var handlerType))
                return new MessageHandlerScope(_container.GetNestedContainer(), handlerType);
            throw new Exception($"Handler not found for {reqType.Name}");
        }
    }
}