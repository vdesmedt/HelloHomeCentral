using System.Collections.Generic;
using System.Reflection;
using HelloHome.Central.Common;
using HelloHome.Central.Domain.Entities;

namespace HelloHome.Central.Hub.Logic
{
    public static class NodeLogger
    {           
        public static ITimeProviderFactory TimeProviderFactory { private get; set; }
        
        public static void AddLog(this Node node, string type, string data = null)
        {
            var timeProvider = TimeProviderFactory.Create();
            if(node.Logs == null)
                node.Logs = new List<NodeLog>();
            node.Logs.Add(new NodeLog { Time = timeProvider.UtcNow, NodeId = node.Id, Type = type, Data = data });
            TimeProviderFactory.Release(timeProvider);
        }                  
    }
}