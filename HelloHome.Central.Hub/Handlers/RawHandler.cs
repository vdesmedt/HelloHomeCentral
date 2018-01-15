using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelloHome.Central.Domain;
using HelloHome.Central.Hub.MessageChannel.Messages;
using HelloHome.Central.Hub.MessageChannel.Messages.Reports;
using NLog;

namespace HelloHome.Central.Hub.Handlers
{
    public class RawHandler : MessageHandler<RawReport>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        
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