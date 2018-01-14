using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelloHome.Central.Common;
using HelloHome.Central.Common.Exceptions;
using HelloHome.Central.Domain;
using HelloHome.Central.Domain.Entities;
using HelloHome.Central.Domain.Entities.Includes;
using HelloHome.Central.Hub.Commands;
using HelloHome.Central.Hub.MessageChannel.Messages;
using HelloHome.Central.Hub.MessageChannel.Messages.Reports;
using HelloHome.Central.Hub.Queries;

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
            var node = await _findNodeQuery.ByRfIdAsync(request.FromRfAddress, NodeInclude.AggregatedData);
            if(node == null)
                throw new NodeNotFoundException(request.FromRfAddress);
            
            _touchNode.Touch(node, request.Rssi);
            node.NodeHistory = new List<NodeHistory>
            {
                new EnvironmentDataHistory
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