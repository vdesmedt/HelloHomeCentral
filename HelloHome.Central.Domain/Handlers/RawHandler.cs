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
    public class RawHandler(ILogger<RawHandler> logger, IUnitOfWork dbCtx) : MessageHandler<RawReport>(dbCtx)
    {
        protected override Task HandleAsync(RawReport request, IList<OutgoingMessage> outgoingMessages, CancellationToken cToken)
        {
            logger.LogWarning("Raw message : {bytes}", BitConverter.ToString(request.Bytes));
            return Task.CompletedTask;
        }
    }
}