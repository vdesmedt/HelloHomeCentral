﻿using System.Collections.Generic;
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
        private readonly string[] _portNames = new string[] {"???", "HAL1", "HAL2", "DRY"};


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

            for (byte i = 0; i < 3; i++)
            {
                if (request.NewPulses[i] > 0)
                {
                    var port = node.Ports.OfType<PulseSensorPort>().SingleOrDefault(_ => _.PortNumber == i);
                    if (port == null)
                        node.Ports.Add(port = new PulseSensorPort()
                        {
                            Node = node,
                            PortNumber = i,
                            PulseCount = 0,
                        });
                    port.PulseCount += request.NewPulses[i];
                    port.PulseHistory = new List<PulseHistory>
                    {
                        new PulseHistory
                        {
                            Node = node,
                            Timestamp = _timeProvider.UtcNow,
                            Rssi = request.Rssi,
                            NewPulses = request.NewPulses[i],
                            Total = port.PulseCount,
                        }
                    };
                }
            }
        }
    }
}