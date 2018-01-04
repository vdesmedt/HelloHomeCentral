using HelloHome.Central.Hub.MessageChannel.Messages;

namespace HelloHome.Central.Hub.Handlers.Factory
{
    public interface IMessageHandlerFactory
    {
        IMessageHandler Create(IncomingMessage request);
        void Release(IMessageHandler messageHandler);
    }
}