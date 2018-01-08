using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelloHome.Central.Domain.Entities;
using HelloHome.Central.Domain.Entities.Includes;
using HelloHome.Central.Hub.Commands;
using HelloHome.Central.Hub.MessageChannel.Messages.Commands;
using HelloHome.Central.Hub.MessageChannel.Messages.Reports;
using HelloHome.Central.Hub.Queries;
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
            RegisterDbContext(nameof(NodeStart_CreateNode_When_SignatureNotFound));
            
            var nodeStartedReport = new NodeStartedReport
            {
                FromRfAddress = 2,
                Rssi = -90,
                Signature = long.MaxValue,
                Version = "1234567",      
            };
            
            var responses = await Hub.ProcessOne(nodeStartedReport, _cts.Token);                                    

            //Node is properly created in Db
            Assert.Equal(1, DbCtx.Nodes.Count());
            var dbNode = DbCtx.Nodes.Single();            
            Assert.True(dbNode.RfAddress > 0);
            Assert.Equal("1234567", dbNode.Metadata.Version);
            Assert.NotNull(dbNode.Logs);
            Assert.Equal(2, dbNode.Logs.Count);
            Assert.Contains(dbNode.Logs, _ => _.Type == "CRTD");
            Assert.Contains(dbNode.Logs, _ => _.Type == "STRT");
            
            //Config message is correct
            Assert.Equal(1, responses.Count);
            Assert.IsType<NodeConfigCommand>(responses[0]);
            var configCommand = responses[0] as NodeConfigCommand;
            Assert.Equal(nodeStartedReport.Signature, configCommand.Signature);
            Assert.Equal(nodeStartedReport.FromRfAddress, configCommand.ToRfAddress);
            Assert.Equal(dbNode.RfAddress, configCommand.NewRfAddress);
        }

        [Fact]
        public async Task NodeStart_ReuseRfADdress_When_Exists()
        {
            RegisterDbContext(nameof(NodeStart_ReuseRfADdress_When_Exists));
            DbCtx.Nodes.Add(new Node() { RfAddress = 3, Signature = long.MaxValue });
            DbCtx.SaveChanges();
            var nodeStartedReport = new NodeStartedReport
            {
                FromRfAddress = 3,
                Rssi = -90,
                Signature = long.MaxValue,
                Version = "1234567",      
            };
            
            var responses = await Hub.ProcessOne(nodeStartedReport, _cts.Token);                                    
            
            Assert.Equal(1, responses.Count);
            Assert.IsType<NodeConfigCommand>(responses[0]);
            var configCommand = responses[0] as NodeConfigCommand;

            Assert.Equal(nodeStartedReport.Signature, configCommand.Signature);
            Assert.Equal(3, configCommand.ToRfAddress);
            Assert.Equal(3, configCommand.NewRfAddress);
            
        }
    }
}