using System.Collections.Generic;
using System.Reflection;
using HelloHome.Central.Common;
using HelloHome.Central.Domain.Entities;

namespace HelloHome.Central.Hub.Logic
{
    public interface INodeLogger
    {
        void Log(Node node, string type, string data = null);
    }

    public class NodeLogger : INodeLogger
    {
        private readonly ITimeProvider _timeProvider;

        public NodeLogger(ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }
        public void Log(Node node, string type, string data = null)
        {
            if(node.Logs == null)
                node.Logs = new List<NodeLog>();
            node.Logs.Add(new NodeLog { Time = _timeProvider.UtcNow, NodeId = node.Id, Type = type, Data = data });
        }
    }
}