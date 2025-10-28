using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelloHome.Central.Domain.Handlers.Base;
using HelloHome.Central.Domain.Messages;
using HelloHome.Central.Domain.Messages.Commands;
using HelloHome.Central.Domain.Messages.Reports;

namespace HelloHome.Central.Domain.Handlers
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