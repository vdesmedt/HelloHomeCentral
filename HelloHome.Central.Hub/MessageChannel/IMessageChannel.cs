using System.Threading;
using System.Threading.Tasks;
using HelloHome.Central.Hub.MessageChannel.Messages;

namespace HelloHome.Central.Hub.MessageChannel
{
    public interface IMessageChannel
    {
        Task<IncomingMessage> TryReadNextAsync(CancellationToken cancellationToken);
        Task SendAsync(OutgoingMessage message, CancellationToken cancellationToken);
    }
}