using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelloHome.Central.Domain.Messages;

namespace HelloHome.Central.Domain.Handlers.Base
{
    public interface IMessageHandler
    {
        Task<IList<OutgoingMessage>> HandleAsync(IncomingMessage request, CancellationToken cToken);
    }
}