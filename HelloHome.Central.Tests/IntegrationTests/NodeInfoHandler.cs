using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelloHome.Central.Domain.Entities;
using HelloHome.Central.Hub.MessageChannel.Messages.Commands;
using HelloHome.Central.Hub.MessageChannel.Messages.Reports;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HelloHome.Central.Tests.IntegrationTests
{
    public class NodeInfoHandler : IntegrationTestBase
    {
        private CancellationTokenSource _cts;

        public NodeInfoHandler()
        {
            _cts = new CancellationTokenSource();
        }
        
        [Fact]
        public async Task SimplePass()
        {
            var dBctx = RegisterDbContext(nameof(SimplePass));
            
            //Create Node
            var outMsgs = await Hub.ProcessOne(new NodeStartedReport { FromRfAddress = 1, NodeType = NodeType.HelloNergie, Signature = long.MaxValue/2, Version = "VER1.0"}, _cts.Token);
            var config = outMsgs.Cast<NodeConfigCommand>().Single();
            
            //NodeInfo
            await Hub.ProcessOne(new NodeInfoReport { FromRfAddress = config.NewRfAddress, SendErrorCount = 34, Voltage = 3.7f}, _cts.Token);

            
            var dbNode = dBctx.Nodes.Include(_ => _.AggregatedData).Single(_ => _.RfAddress == config.NewRfAddress);
            
            Assert.Equal(34, dbNode.AggregatedData.SendErrorCount);
            Assert.Equal(3.7f, dbNode.AggregatedData.VIn);
            
        }
    }
}