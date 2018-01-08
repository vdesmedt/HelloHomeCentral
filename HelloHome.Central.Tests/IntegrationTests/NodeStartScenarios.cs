using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelloHome.Central.Domain.Entities;
using HelloHome.Central.Domain.Entities.Includes;
using HelloHome.Central.Hub.Commands;
using HelloHome.Central.Hub.MessageChannel.Messages.Reports;
using HelloHome.Central.Hub.Queries;
using Moq;
using NLog.Time;
using Xunit;

namespace HelloHome.Central.Tests.IntegrationTests
{
    public class NodeStartScenarios : IClassFixture<IntegrationTestFixture>
    {
        private readonly IntegrationTestFixture _integrationTestFixture;
        private CancellationTokenSource _cts;

        public NodeStartScenarios(IntegrationTestFixture integrationTestFixture)
        {
            _integrationTestFixture = integrationTestFixture;
            _cts = new CancellationTokenSource();
        }

        [Fact]
        public async Task NodeStartCreateNodeIfSignatureNotFound()
        {
            var nodeStartedReport = new NodeStartedReport
            {
                FromRfAddress = 2,
                Rssi = -90,
                Signature = long.MaxValue,
                Version = "1234567",      
            };

            _integrationTestFixture.RegisterMock<IFindNodeQuery>()
                .Setup(_ => _.BySignatureAsync(nodeStartedReport.Signature, It.IsAny<NodeInclude>()))
                .ReturnsAsync((Node)null);
            _integrationTestFixture.RegisterMock<ICreateNodeCommand>()
                .Setup(_ => _.ExecuteAsync(nodeStartedReport.Signature, It.IsAny<byte>()))
                .ReturnsAsync(new Node { Metadata = new NodeMetadata(), AggregatedData = new NodeAggregatedData()});
                
            _integrationTestFixture.RegisterMock<IListRfIdsQuery>()
                .Setup(_ => _.Execute()).Returns(new List<byte> {1});
            
            var responses = await _integrationTestFixture.Hub.ProcessOne(nodeStartedReport, _cts.Token);
        }
    }
}