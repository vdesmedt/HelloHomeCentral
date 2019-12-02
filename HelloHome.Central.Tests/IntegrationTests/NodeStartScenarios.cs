using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelloHome.Central.Common;
using HelloHome.Central.Domain.Entities;
using HelloHome.Central.Domain.Entities.Includes;
using HelloHome.Central.Hub.Commands;
using HelloHome.Central.Hub.MessageChannel.Messages.Commands;
using HelloHome.Central.Hub.MessageChannel.Messages.Reports;
using Moq;
using NLog.Time;
using Xunit;

namespace HelloHome.Central.Tests.IntegrationTests
{
    public class NodeStartScenarios : IntegrationTestBase
    {
        private readonly CancellationTokenSource _cts;

        public NodeStartScenarios() : base()
        {
            _cts = new CancellationTokenSource();
        }

        [Fact]
        public async Task NodeStart_CreateNode_When_SignatureNotFound()
        {
            var dbCtx = RegisterDbContext(nameof(NodeStart_CreateNode_When_SignatureNotFound));

            var nodeStartedReport = new NodeStartedReport
            {
                FromRfAddress = 2,
                Rssi = -90,
                Signature = long.MaxValue,
                Version = "1234567",
            };

            var responses = await Hub.ProcessOne(nodeStartedReport, _cts.Token);

            //Node is properly created in Db
            Assert.Equal(1, dbCtx.Nodes.Count());
            var dbNode = dbCtx.Nodes.Single();
            Assert.True(dbNode.RfAddress > 0);
            Assert.Equal("1234567", dbNode.Metadata.Version);

            //Config message is correct
            Assert.Equal(1, responses.Count);
            var configCommand = Assert.IsType<NodeConfigCommand>(responses[0]);
            Assert.Equal(nodeStartedReport.Signature, configCommand.Signature);
            Assert.Equal(nodeStartedReport.FromRfAddress, configCommand.ToRfAddress);
            Assert.Equal(dbNode.RfAddress, configCommand.NewRfAddress);
        }

        [Fact]
        public async Task NodeStart_ReuseRfADdress_When_Exists()
        {
            var dbCtx = RegisterDbContext(nameof(NodeStart_ReuseRfADdress_When_Exists));
            dbCtx.Nodes.Add(new Node()
            {
                RfAddress = 3,
                Signature = long.MaxValue,
                AggregatedData = new NodeAggregatedData { },
                Metadata = new NodeMetadata { }
            });
            dbCtx.SaveChanges();
            var nodeStartedReport = new NodeStartedReport
            {
                FromRfAddress = 3,
                Rssi = -90,
                Signature = long.MaxValue,
                Version = "1234567",
            };

            var responses = await Hub.ProcessOne(nodeStartedReport, _cts.Token);

            Assert.Equal(1, responses.Count);
            var configCommand = Assert.IsType<NodeConfigCommand>(responses[0]);

            Assert.Equal(nodeStartedReport.Signature, configCommand.Signature);
            Assert.Equal(3, configCommand.ToRfAddress);
            Assert.Equal(3, configCommand.NewRfAddress);
        }

        [Fact]
        public async Task NodeStart_Update_LastSeen_LastStartupTime()
        {
            var dbCtx = RegisterDbContext(nameof(NodeStart_Update_LastSeen_LastStartupTime));

            var nodeStartedReport = new NodeStartedReport
            {
                FromRfAddress = 3,
                Rssi = -90,
                Signature = long.MaxValue,
                Version = "1234567",
            };

            var ancianTime = DateTime.UtcNow.AddDays(-1);
            var modernTime = DateTime.UtcNow;

            dbCtx.Nodes.Add(new Node()
            {
                RfAddress = nodeStartedReport.FromRfAddress,
                Signature = nodeStartedReport.Signature,
                AggregatedData = new NodeAggregatedData {StartupTime = ancianTime},
                Metadata = new NodeMetadata { }
            });
            dbCtx.SaveChanges();

            RegisterMock<ITimeProvider>()
                .Setup(_ => _.UtcNow).Returns(modernTime);

            await Hub.ProcessOne(nodeStartedReport, _cts.Token);

            var dbNode = dbCtx.Nodes.Single(x => x.Signature == nodeStartedReport.Signature);

            Assert.Equal(modernTime, dbNode.AggregatedData.StartupTime);
            Assert.Equal(modernTime, dbNode.LastSeen);
        }
    }
}