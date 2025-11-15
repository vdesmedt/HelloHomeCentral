using System;
using HelloHome.Central.Common;
using HelloHome.Central.Domain.CmdQrys.Base;
using HelloHome.Central.Domain.Entities;
using Microsoft.Extensions.Logging;


namespace HelloHome.Central.Domain.Logic
{
    public interface ITouchNode : ICommand
    {
        void Touch(Node node, int rssi);
    }

    public class TouchNode(ILogger<TouchNode> logger, ITimeProvider timeProvider) : ITouchNode
    {
        public void Touch(Node node, int rssi)
        {
            if(node.AggregatedData == default(NodeAggregatedData))
                throw new ArgumentException("node entity should be loaded with its aggregated data for Touch to work");
            node.LastSeen = timeProvider.UtcNow;
            node.AggregatedData.Rssi = rssi;
            node.AggregatedData.MaxUpTime =
                TimeSpan.FromDays(
                    Math.Max(
                        node.AggregatedData.MaxUpTime.TotalDays,
                        (timeProvider.UtcNow - node.AggregatedData.StartupTime).TotalDays
                    )
                );
            logger.LogDebug("Node with signature {signature} was touched",node.Signature);
        }
    }
}