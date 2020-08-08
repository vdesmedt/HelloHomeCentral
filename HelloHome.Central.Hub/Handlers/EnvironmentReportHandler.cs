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
    public class EnvironmentReportHandler : MessageHandler<EnvironmentalReport>
    {
        private readonly IFindNodeQuery _findNodeQuery;
        private readonly ITouchNode _touchNode;
        private readonly ITimeProvider _timeProvider;

        public EnvironmentReportHandler(
            IUnitOfWork dbCtx,
            IFindNodeQuery findNodeQuery,
            ITouchNode touchNode,
            ITimeProvider timeProvider)
            : base(dbCtx)
        {
            _findNodeQuery = findNodeQuery;
            _touchNode = touchNode;
            _timeProvider = timeProvider;
        }

        protected override async Task HandleAsync(EnvironmentalReport request, IList<OutgoingMessage> outgoingMessages, CancellationToken cToken)
        {
            var node = await _findNodeQuery.ByRfIdAsync(request.FromRfAddress, NodeInclude.AggregatedData | NodeInclude.Ports);
            if(node == null)
                throw new NodeNotFoundException(request.FromRfAddress);
            _touchNode.Touch(node, request.Rssi);
            var envPort = node.Ports.OfType<EnvironmentSensorPort>().SingleOrDefault();
            if (envPort == null)
            {
                envPort = new EnvironmentSensorPort
                {
                    PortNumber = (byte)ReservedPortNumber.Environment,
                    Name = "Env",
                    UpdateFrequency = 1,
                };
                node.Ports.Add(envPort);
            }

            envPort.History = new List<EnvironmentHistory>
            {
                new EnvironmentHistory
                {
                    Humidity = request.Humidity,
                    Temperature = request.Temperature,
                    Pressure = request.Pressure,
                    Rssi = request.Rssi,
                    Timestamp = _timeProvider.UtcNow                   
                }
            };
            node.AggregatedData.Humidity = request.Humidity;
            node.AggregatedData.Temperature = request.Temperature;
            node.AggregatedData.AtmosphericPressure = request.Pressure;
        }
    }
}