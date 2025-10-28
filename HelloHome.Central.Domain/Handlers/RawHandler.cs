using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelloHome.Central.Domain.Handlers.Base;
using HelloHome.Central.Domain.Messages;
using HelloHome.Central.Domain.Messages.Reports;
using NLog;

namespace HelloHome.Central.Domain.Handlers
{
    public class RawHandler : MessageHandler<RawReport>
    {
        private static readonly Logger Logger = LogManager.GetLogger(nameof(RawHandler));
        
        public RawHandler(IUnitOfWork dbCtx) : base(dbCtx)
        {
        }

        protected override Task HandleAsync(RawReport request, IList<OutgoingMessage> outgoingMessages, CancellationToken cToken)
        {
            Logger.Warn(() => $"Raw message : {BitConverter.ToString(request.Bytes)}");
            return Task.CompletedTask;
        }
    }
}