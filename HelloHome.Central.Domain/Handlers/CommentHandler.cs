using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelloHome.Central.Domain.Handlers.Base;
using HelloHome.Central.Domain.Messages;
using HelloHome.Central.Domain.Messages.Reports;
using Microsoft.Extensions.Logging;

namespace HelloHome.Central.Domain.Handlers
{
    public class CommentHandler(ILogger<CommentHandler> logger, IUnitOfWork dbCtx)
        : MessageHandler<CommentReport>(dbCtx)
    {
        protected override Task HandleAsync(CommentReport request, IList<OutgoingMessage> outgoingMessages, CancellationToken cToken)
        {
            logger.LogInformation(request.Comment);
            return Task.CompletedTask;
        }
    }
}