using HelloHome.Central.Hub.NodeBridge;
using HelloHome.Central.Hub.NodeBridge.Performance;
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