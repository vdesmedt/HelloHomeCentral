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
    public class PongHandler(IUnitOfWork dbCtx) : MessageHandler<PongReport>(dbCtx)
    {
        private static readonly Logger Logger = LogManager.GetLogger(nameof(PongHandler));

        protected override Task HandleAsync(PongReport request, IList<OutgoingMessage> outgoingMessages, CancellationToken cToken)
        {
            var elapsed = (long)((DateTimeOffset.Now - DateTimeOffset.Now.Date).TotalMilliseconds - request.MillisIn);
            Logger.Info($"Ping returned from {request.FromRfAddress} after {elapsed} ms. Rssi is {request.PingRssi}/{request.Rssi}, Millis on Device is {request.MillisOut}");
            return Task.CompletedTask;
        }
    }
}