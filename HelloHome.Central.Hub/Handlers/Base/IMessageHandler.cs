using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelloHome.Central.Hub.MessageChannel.Messages;

namespace HelloHome.Central.Hub.Handlers.Base
{
    public interface IMessageHandler
    {
        Task<IList<OutgoingMessage>> HandleAsync(IncomingMessage request, CancellationToken cToken);
    }
}