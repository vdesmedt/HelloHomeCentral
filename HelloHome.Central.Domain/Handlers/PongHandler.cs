using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelloHome.Central.Domain.Handlers.Base;
using HelloHome.Central.Domain.Messages;
using HelloHome.Central.Domain.Messages.Reports;
using Microsoft.Extensions.Logging;

namespace HelloHome.Central.Domain.Handlers
{
    public class PongHandler(ILogger<PongHandler> logger, IUnitOfWork dbCtx) : MessageHandler<PongReport>(dbCtx)
    {
        protected override Task HandleAsync(PongReport request, IList<OutgoingMessage> outgoingMessages, CancellationToken cToken)
        {
            var elapsed = (long)((DateTimeOffset.Now - DateTimeOffset.Now.Date).TotalMilliseconds - request.MillisIn);
            logger.LogInformation(
                "Ping returned from {rfAdr} after {elapsed} ms. Rssi is {PingRssi}/{Rssi}, Millis on Device is {MillisOut}", 
                request.FromRfAddress, elapsed, request.PingRssi, request.Rssi, request.MillisOut);
            return Task.CompletedTask;
        }
    }
}