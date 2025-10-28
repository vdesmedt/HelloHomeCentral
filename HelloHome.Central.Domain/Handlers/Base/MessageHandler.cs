using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelloHome.Central.Domain.Messages;

namespace HelloHome.Central.Domain.Handlers.Base
{
    public abstract class MessageHandler<T> : IMessageHandler where T : IncomingMessage
    {
		readonly IUnitOfWork _dbCtx;
        
        protected MessageHandler (IUnitOfWork dbCtx)
		{
			_dbCtx = dbCtx;
		}

        public async Task<IList<OutgoingMessage>> HandleAsync(IncomingMessage request, CancellationToken cToken)
        {
            if(request.GetType() != typeof(T))
                throw new ArgumentException($"request of type {request.GetType().Name} cannot be processed by {this.GetType().Name}");
            var outgoigMessages = new List<OutgoingMessage>();
            await HandleAsync((T) request, outgoigMessages, cToken);
            await _dbCtx.CommitAsync();
            return outgoigMessages;
        }

        protected abstract Task HandleAsync(T request, IList<OutgoingMessage> outgoingMessages, CancellationToken cToken);
    }
}