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
using NLog;

namespace HelloHome.Central.Hub.Handlers
{
    public class NodeInfoHandler : MessageHandler<NodeInfoReport>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IFindNodeQuery _findNodeQuery;
        private readonly ITouchNode _touchNode;

        public NodeInfoHandler(IUnitOfWork dbCtx, IFindNodeQuery findNodeQuery, ITouchNode touchNode) : base(dbCtx)
        {
            _findNodeQuery = findNodeQuery;
            _touchNode = touchNode;
        }

        protected override async Task HandleAsync(NodeInfoReport request, IList<OutgoingMessage> outgoingMessages, CancellationToken cToken)
        {
            var node = await _findNodeQuery.ByRfIdAsync(request.FromRfAddress);
            if (node == null)
                throw new NodeNotFoundException(request.FromRfAddress);            
            _touchNode.Touch(node, request.Rssi);

            node.AggregatedData.SendErrorCount = request.SendErrorCount;
            node.AggregatedData.VIn = request.Voltage;

            var nodeHealthHistory = new NodeHealthHistory
            {
                Timestamp = TimeProvider.Current.UtcNow,
                Rssi = request.Rssi,
                SendErrorCount = request.SendErrorCount,
                VIn = request.Voltage,                
            };
            node.NodeHistory.Add(nodeHealthHistory);
        }
    }
}