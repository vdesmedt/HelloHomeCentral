using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelloHome.Central.Common;
using HelloHome.Central.Common.Exceptions;
using HelloHome.Central.Domain;
using HelloHome.Central.Domain.Entities;
using HelloHome.Central.Hub.Commands;
using HelloHome.Central.Hub.MessageChannel.Messages;
using HelloHome.Central.Hub.MessageChannel.Messages.Reports;
using HelloHome.Central.Hub.Queries;

namespace HelloHome.Central.Hub.Handlers
{
    public class PulseHandler : MessageHandler<PulseReport>
    {
        readonly IFindNodeQuery _findNodeQuery;
        readonly ITouchNode _touchNode;
        private readonly ITimeProvider _timeProvider;

        public PulseHandler(IUnitOfWork dbCtx, IFindNodeQuery findNodeQuery, ITouchNode touchNode, ITimeProvider timeProvider) : base(dbCtx)
        {
            _touchNode = touchNode;
            _timeProvider = timeProvider;
            _findNodeQuery = findNodeQuery;
        }

        protected override async Task HandleAsync(PulseReport request, IList<OutgoingMessage> outgoingMessages,
            CancellationToken cToken)
        {
            var node = await _findNodeQuery.ByRfIdAsync(request.FromRfAddress);
            if (node == null)
                throw new NodeNotFoundException(request.FromRfAddress);
            _touchNode.Touch(node, request.Rssi);

            var nodePulseHistory = new PulseHistory
            {
                Timestamp = _timeProvider.UtcNow,
                Rssi = request.Rssi,                
                NewPulses = request.NewPulses,                
            };
            node.NodeHistory = new List<NodeHistory> {nodePulseHistory};
        }
    }
}