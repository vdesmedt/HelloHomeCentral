using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelloHome.Central.Common;
using HelloHome.Central.Domain;
using HelloHome.Central.Domain.CmdQrys;
using HelloHome.Central.Domain.Entities;
using HelloHome.Central.Domain.Entities.Includes;
using HelloHome.Central.Domain.Logic;
using HelloHome.Central.Hub.Handlers.Base;
using HelloHome.Central.Hub.MessageChannel.Messages;
using HelloHome.Central.Hub.MessageChannel.Messages.Commands;
using HelloHome.Central.Hub.MessageChannel.Messages.Reports;

namespace HelloHome.Central.Hub.Handlers
{
    public class ActionConfirmedHandler : MessageHandler<ActionConfirmedReport>
    {
        private readonly ITouchNode _touchNode;
        private readonly IFindPortQuery _findPortQuery;
        private readonly ITimeProvider _timeProvider;

        public ActionConfirmedHandler(IUnitOfWork dbCtx, ITouchNode touchNode, IFindPortQuery findPortQuery, ITimeProvider timeProvider) : base(dbCtx)
        {
            _touchNode = touchNode;
            _findPortQuery = findPortQuery;
            _timeProvider = timeProvider;
        }

        protected override async Task HandleAsync(ActionConfirmedReport request, IList<OutgoingMessage> outgoingMessages, CancellationToken cToken)
        {
            if (request.ConfigmedAction is SetRelayStateCommand srCommand)
            {
                var port = await _findPortQuery.ByNodeRfAndPortNumberAsyn(srCommand.ToRfAddress, srCommand.PortNumber, PortInclude.NodeAggregatedData);
                if(port == null)
                    throw new ArgumentException($"Port {srCommand.PortNumber} on Node with rfAdr {srCommand.ToRfAddress} was not found in databse.");
                _touchNode.Touch(port.Node, request.Rssi);
                if (port is RelayActuator raPort)
                {
                    raPort.RelayState = (OnOffState) srCommand.NewState;
                    raPort.History = new List<RelayHistory>
                    {
                        new RelayHistory
                        {
                            Rssi = request.Rssi,
                            Timestamp = _timeProvider.UtcNow,
                            NewRelayState = (OnOffState)srCommand.NewState,
                        }
                    };
                }
                else
                {
                    throw new ArgumentException($"Port {srCommand.PortNumber} on Node with rfAdr {srCommand.ToRfAddress} does not seems to of type {nameof(RelayActuator)}");
                }
            }
        }
    }
}