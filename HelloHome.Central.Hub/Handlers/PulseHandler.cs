using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelloHome.Central.Common;
using HelloHome.Central.Common.Exceptions;
using HelloHome.Central.Domain;
using HelloHome.Central.Domain.CmdQrys;
using HelloHome.Central.Domain.Entities;
using HelloHome.Central.Domain.Entities.Includes;
using HelloHome.Central.Domain.Logic;
using HelloHome.Central.Hub.Handlers.Base;
using HelloHome.Central.Hub.MessageChannel.Messages;
using HelloHome.Central.Hub.MessageChannel.Messages.Reports;

namespace HelloHome.Central.Hub.Handlers
{
    public class PulseHandler : MessageHandler<PulseReport>
    {
        readonly IFindNodeQuery _findNodeQuery;
        readonly ITouchNode _touchNode;
        private readonly ITimeProvider _timeProvider;


        public PulseHandler(IUnitOfWork dbCtx, IFindNodeQuery findNodeQuery, ITouchNode touchNode,
            ITimeProvider timeProvider) : base(dbCtx)
        {
            _touchNode = touchNode;
            _timeProvider = timeProvider;
            _findNodeQuery = findNodeQuery;
        }

        protected override async Task HandleAsync(PulseReport request, IList<OutgoingMessage> outgoingMessages,
            CancellationToken cToken)
        {
            var node = await _findNodeQuery.ByRfIdAsync(request.FromRfAddress,
                NodeInclude.AggregatedData | NodeInclude.Ports);
            if (node == null)
                throw new NodeNotFoundException(request.FromRfAddress);
            _touchNode.Touch(node, request.Rssi);

            var port = node.Ports.OfType<PulseSensor>().SingleOrDefault(_ => _.PortNumber == request.PortNumber);
            if (port == null)
                node.Ports.Add(port = new PulseSensor()
                {
                    Node = node,
                    PortNumber = request.PortNumber,
                    PulseCount = 0,
                });
            port.PulseCount += request.NewPulse;
            port.History = new List<PulseHistory>
            {
                new PulseHistory
                {
                    Timestamp = _timeProvider.UtcNow,
                    Rssi = request.Rssi,
                    NewPulses = request.NewPulse,
                    Total = port.PulseCount,
                }
            };
        }
    }
}