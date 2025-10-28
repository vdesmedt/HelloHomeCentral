using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelloHome.Central.Common;
using HelloHome.Central.Common.Exceptions;
using HelloHome.Central.Domain.CmdQrys;
using HelloHome.Central.Domain.Entities;
using HelloHome.Central.Domain.Entities.Includes;
using HelloHome.Central.Domain.Handlers.Base;
using HelloHome.Central.Domain.Logic;
using HelloHome.Central.Domain.Messages;
using HelloHome.Central.Domain.Messages.Reports;
using NLog;

namespace HelloHome.Central.Domain.Handlers
{
    public class NodeInfoHandler : MessageHandler<NodeInfoReport>
    {
        private static readonly Logger Logger = LogManager.GetLogger(nameof(NodeInfoHandler));

        private readonly IFindNodeQuery _findNodeQuery;
        private readonly ITouchNode _touchNode;
        private readonly ITimeProvider _timeProvider;

        public NodeInfoHandler(IUnitOfWork dbCtx, IFindNodeQuery findNodeQuery, ITouchNode touchNode,
            ITimeProvider timeProvider) : base(dbCtx)
        {
            _findNodeQuery = findNodeQuery;
            _touchNode = touchNode;
            _timeProvider = timeProvider;
        }

        protected override async Task HandleAsync(NodeInfoReport request, IList<OutgoingMessage> outgoingMessages,
            CancellationToken cToken)
        {
            var node = await _findNodeQuery.ByRfIdAsync(request.FromRfAddress,
                NodeInclude.AggregatedData | NodeInclude.Ports);
            if (node == null)
                throw new NodeNotFoundException(request.FromRfAddress);
            _touchNode.Touch(node, request.Rssi);
            var healthPort = node.Ports.OfType<NodeHealthSensor>().SingleOrDefault();
            if (healthPort == null)
            {
                healthPort = new NodeHealthSensor
                {
                    PortNumber = (byte) ReservedPortNumber.NodeHealth,
                    Name = "Health",
                    UpdateFrequency = 1,
                };
                node.Ports.Add(healthPort);
            }

            healthPort.History = new List<NodeHealthHistory>
            {
                new NodeHealthHistory
                {
                    Timestamp = _timeProvider.UtcNow,
                    Rssi = request.Rssi,
                    SendErrorCount = request.SendErrorCount,
                    VIn = request.Voltage,
                }
            };
            node.AggregatedData.SendErrorCount = request.SendErrorCount;
            node.AggregatedData.VIn = request.Voltage;
        }
    }
}