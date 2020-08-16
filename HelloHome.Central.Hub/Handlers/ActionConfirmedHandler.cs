using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelloHome.Central.Domain;
using HelloHome.Central.Domain.CmdQrys;
using HelloHome.Central.Domain.Entities;
using HelloHome.Central.Hub.Handlers.Base;
using HelloHome.Central.Hub.MessageChannel.Messages;
using HelloHome.Central.Hub.MessageChannel.Messages.Commands;
using HelloHome.Central.Hub.MessageChannel.Messages.Reports;

namespace HelloHome.Central.Hub.Handlers
{
    public class ActionConfirmedHandler : MessageHandler<ActionConfirmedReport>
    {
        private readonly IFindPortQuery _findPortQuery;

        public ActionConfirmedHandler(IUnitOfWork dbCtx, IFindPortQuery findPortQuery) : base(dbCtx)
        {
            _findPortQuery = findPortQuery;
        }

        protected override async Task HandleAsync(ActionConfirmedReport request, IList<OutgoingMessage> outgoingMessages, CancellationToken cToken)
        {
            if (request.ConfigmedAction is SetRelayStateCommand srCommand)
            {
                var port = await _findPortQuery.ByNodeRfAndPortNumber(srCommand.ToRfAddress, srCommand.PortNumber);
                if (port is RelayActuatorPort raPort)
                {
                    raPort.RelayState = (OnOffState) srCommand.NewState;
                }
                else
                {
                    throw new ArgumentException($"Port {srCommand.PortNumber} on Node with rfAdr {srCommand.ToRfAddress} does not seems to of type {nameof(RelayActuatorPort)}");
                }
            }
        }
    }
}