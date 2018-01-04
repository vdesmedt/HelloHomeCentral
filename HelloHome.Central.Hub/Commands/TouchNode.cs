﻿using System;
using System.Threading.Tasks;
using HelloHome.Central.Common;
using HelloHome.Central.Domain.Entities;
using NLog;

namespace HelloHome.Central.Hub.Commands
{
    public interface ITouchNode : ICommand
    {
        Task TouchAsync(Node node, int rssi);
    }

    public class TouchNode : ITouchNode
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ();

        private readonly ITimeProvider _timeProvider;

        public TouchNode(ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }

        public Task TouchAsync(Node node, int rssi)
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
            Logger.Debug("Node with signature {0} was touched", node.Signature);
            return Task.FromResult(0);
        }
    }
}