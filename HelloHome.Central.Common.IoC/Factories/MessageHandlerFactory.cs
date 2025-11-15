using System.Collections.Concurrent;
using HelloHome.Central.Domain.Handlers.Base;
using HelloHome.Central.Domain.Messages;
using JetBrains.Annotations;
using Lamar;

namespace HelloHome.Central.Common.IoC.Factories
{
    public interface IMessageHandlerFactory
    {
        IMessageHandler Build(Message message);
        MessageHandlerScope BuildInNestedScope(Message message);
    }

    public class MessageHandlerScope(INestedContainer nestedContainer, Type handlerType) : IDisposable
    {
        public readonly IMessageHandler Handler = (IMessageHandler)nestedContainer.GetInstance(handlerType);

        public void Dispose()
        {
            nestedContainer.Dispose();
        }
    }

    [UsedImplicitly]
    public class MessageHandlerFactory : IMessageHandlerFactory
    {
        private readonly IContainer _container;
        private readonly ConcurrentDictionary<Type, Type> _typeMap;

        public MessageHandlerFactory(IContainer container)
        {
            _container = container;
            var handlerTypes = container.Model.AllInstances
                .Where(_ => _.ServiceType == typeof(IMessageHandler))
                .Select(_ => _.ImplementationType)
                .Distinct();
            
            _typeMap = new ConcurrentDictionary<Type, Type>(handlerTypes.Select(t =>
                new KeyValuePair<Type, Type>(t.BaseType.GenericTypeArguments[0], t)));
        }

        public IMessageHandler Build(Message message)
        {
            var reqType = message.GetType();
            if (_typeMap.TryGetValue(reqType, out var handlerType))
                return (IMessageHandler) _container.GetInstance(handlerType);
            throw new Exception($"Handler not found for {reqType.Name}");
        }

        public MessageHandlerScope BuildInNestedScope(Message message)
        {
            var reqType = message.GetType();
            if (_typeMap.TryGetValue(reqType, out var handlerType))
                return new MessageHandlerScope(_container.GetNestedContainer(), handlerType);
            throw new Exception($"Handler not found for {reqType.Name}");
        }
    }
}