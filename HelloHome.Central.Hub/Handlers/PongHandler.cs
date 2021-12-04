using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelloHome.Central.Domain;
using HelloHome.Central.Hub.Handlers.Base;
using HelloHome.Central.Hub.MessageChannel.Messages;
using HelloHome.Central.Hub.MessageChannel.Messages.Reports;
using NLog;

namespace HelloHome.Central.Hub.Handlers
{
    public class PongHandler :MessageHandler<PongReport>
    {
        private static readonly Logger Logger = LogManager.GetLogger(nameof(PongHandler));
        
        public PongHandler(IUnitOfWork dbCtx) : base(dbCtx)
        {
        }

        protected override async Task HandleAsync(PongReport request, IList<OutgoingMessage> outgoingMessages, CancellationToken cToken)
        {
            var ellapsed = (long)((DateTimeOffset.Now - DateTimeOffset.Now.Date).TotalMilliseconds - request.MillisIn);
            Logger.Info($"Ping returned from {request.FromRfAddress} after {ellapsed} ms. Rssi is {request.PingRssi}/{request.Rssi}, Millis on Device is {request.MillisOut}");
        }
    }
}