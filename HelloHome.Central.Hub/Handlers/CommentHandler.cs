using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelloHome.Central.Domain;
using HelloHome.Central.Hub.MessageChannel.Messages;
using HelloHome.Central.Hub.MessageChannel.Messages.Reports;
using NLog;

namespace HelloHome.Central.Hub.Handlers
{
    public class CommentHandler : MessageHandler<CommentReport>
    {		
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
   
		public CommentHandler (IUnitOfWork dbCtx) : base(dbCtx)
		{

		}

		protected override async Task HandleAsync(CommentReport request, IList<OutgoingMessage> outgoingMessages, CancellationToken cToken)
        {
            Logger.Info(request.Comment);
            await Task.Yield();
        }
    }
}