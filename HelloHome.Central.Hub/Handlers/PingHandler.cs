using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelloHome.Central.Domain;
using HelloHome.Central.Hub.Handlers.Base;
using HelloHome.Central.Hub.MessageChannel.Messages;
using HelloHome.Central.Hub.MessageChannel.Messages.Commands;
using HelloHome.Central.Hub.MessageChannel.Messages.Reports;

namespace HelloHome.Central.Hub.Handlers
{
    public class PingHandler :MessageHandler<PingReport>
    {
        public PingHandler(IUnitOfWork dbCtx) : base(dbCtx)
        {
        }

        protected override Task HandleAsync(PingReport request, IList<OutgoingMessage> outgoingMessages, CancellationToken cToken)
        {
            outgoingMessages.Add(new PongCommand { ToRfAddress = request.FromRfAddress, Millis = request.Millis, ReceiveRssi = request.Rssi});
            return Task.CompletedTask;
        }
    }
}