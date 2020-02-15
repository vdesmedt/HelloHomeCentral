using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelloHome.Central.Hub.MessageChannel.Messages;

namespace HelloHome.Central.Hub.NodeBridge
{
    public interface INodeBridge
    {
        long LeftToProcess { get; }
        void Send(OutgoingMessage message);
        Task Communication(CancellationToken cancellationToken);
        Task Processing(CancellationToken cancellationToken);
        Task<IList<OutgoingMessage>> ProcessOne(IncomingMessage msg, CancellationToken token);
    }
}