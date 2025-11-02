using HelloHome.Central.Common.Performance;
using HelloHome.Central.Hub.NodeBridge;
using Lamar;
using Microsoft.Extensions.DependencyInjection;

namespace HelloHome.Central.Hub.IoC.Registries
{
    public class NodeBridgeRegistry : ServiceRegistry
    {
        public NodeBridgeRegistry()
        {
            this.AddSingleton<INodeBridge, NodeBridge.NodeBridge>();
            this.AddSingleton<IPerformanceStats, PerformanceStats>();
        }
    }
}