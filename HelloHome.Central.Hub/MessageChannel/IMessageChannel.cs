using System.Threading;
using HelloHome.Central.Hub.MessageChannel.Messages;

namespace HelloHome.Central.Hub.MessageChannel
{
    public interface IMessageChannel
    {
        IncomingMessage TryReadNext(int millisecond, CancellationToken cancellationToken);
        void Send(OutgoingMessage message);
    }
}