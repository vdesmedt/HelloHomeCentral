using System;
using HelloHome.Central.Common;
using HelloHome.Central.Domain.CmdQrys.Base;
using HelloHome.Central.Domain.Entities;
using NLog;

namespace HelloHome.Central.Domain.Logic
{
    public interface ITouchNode : ICommand
    {
        void Touch(Node node, int rssi);
    }

    public class TouchNode : ITouchNode
    {
        private static readonly Logger Logger = LogManager.GetLogger(nameof(TouchNode));

        private readonly ITimeProvider _timeProvider;

        public TouchNode(ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }

        public void Touch(Node node, int rssi)
        {
            if(node.AggregatedData == default(NodeAggregatedData))
                throw new ArgumentException("node entity should be loaded with its aggregated data for Touch to work");
            node.LastSeen = _timeProvider.UtcNow;
            node.AggregatedData.Rssi = rssi;
            node.AggregatedData.MaxUpTime =
                TimeSpan.FromDays(
                    Math.Max(
                        node.AggregatedData.MaxUpTime.TotalDays,
                        (_timeProvider.UtcNow - node.AggregatedData.StartupTime).TotalDays
                    )
                );
            Logger.Debug(() => $"Node with signature {node.Signature} was touched");
        }
    }
}