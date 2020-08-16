using System;
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
using HelloHome.Central.Domain.Logic.CoreLogic;
using HelloHome.Central.Hub.Handlers.Base;
using HelloHome.Central.Hub.MessageChannel.Messages;
using HelloHome.Central.Hub.MessageChannel.Messages.Commands;
using HelloHome.Central.Hub.MessageChannel.Messages.Reports;
using Action = HelloHome.Central.Domain.Entities.Action;

namespace HelloHome.Central.Hub.Handlers
{
    public class PushButtonPressedHandler : MessageHandler<PushButtonPressedReport>
    {
        private readonly IFindNodeQuery _findNodeQuery;
        private readonly IFindTriggersForPortQuery _findTriggersForPortQuery;
        private readonly ITouchNode _touchNode;
        private readonly ITimeProvider _timeProvider;
        private readonly ICoreLogic _coreLogic;
        private readonly IActionToCommandMapper _actionToCommandMapper;

        public PushButtonPressedHandler(IUnitOfWork dbCtx, IFindNodeQuery findNodeQuery,
            IFindTriggersForPortQuery findTriggersForPortQuery, ITouchNode touchNode,
            ITimeProvider timeProvider, ICoreLogic coreLogic, IActionToCommandMapper actionToCommandMapper) : base(dbCtx)
        {
            _findNodeQuery = findNodeQuery;
            _findTriggersForPortQuery = findTriggersForPortQuery;
            _touchNode = touchNode;
            _timeProvider = timeProvider;
            _coreLogic = coreLogic;
            _actionToCommandMapper = actionToCommandMapper;
        }

        protected override async Task HandleAsync(PushButtonPressedReport request,
            IList<OutgoingMessage> outgoingMessages, CancellationToken cToken)
        {
            var node = await _findNodeQuery.ByRfIdAsync(request.FromRfAddress,
                NodeInclude.AggregatedData | NodeInclude.Ports);
            if (node == null)
                throw new NodeNotFoundException(request.FromRfAddress);
            _touchNode.Touch(node, request.Rssi);

            var port = node.Ports.OfType<PushButtonSensor>().SingleOrDefault(_ => _.PortNumber == request.PortNumber);
            if (port == null)
            {
                if (request.PortNumber < (byte) ReservedPortNumber.Last)
                    throw new ArgumentException($"PortNumber {request.PortNumber} is reserved.");
                if (node.Ports.Any(x => x.PortNumber == request.PortNumber))
                    throw new ArgumentException("A port already exist with that number that is not of type PushButton");
                node.Ports.Add(port = new PushButtonSensor()
                {
                    Node = node,
                    PortNumber = request.PortNumber,
                });
            }

            port.LastPressStyle = request.PressStyle;
            port.History = new List<PushButtonHistory>
            {
                new PushButtonHistory()
                {
                    Timestamp = _timeProvider.UtcNow,
                    Rssi = request.Rssi,
                    PressStyle = request.PressStyle,
                }
            };

            //Check for trigger
            var triggers = await _findTriggersForPortQuery.ByPortIdAsync<PushTrigger>(port.Id, cToken);
            foreach (var trigger in triggers)
            {
                if (trigger.PressStyle == request.PressStyle)
                {
                    var actions = _coreLogic.GetActionsFor(trigger);
                    foreach(var a in actions)
                        outgoingMessages.Add(_actionToCommandMapper.Map(a));
                }
            }
        }
    }
}