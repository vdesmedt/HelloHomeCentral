using HelloHome.Central.Hub.NodeBridge;
using Lamar;
using Microsoft.Extensions.DependencyInjection;

namespace HelloHome.Central.Hub.IoC.Installers
{
    public class NodeBridgeInstaller : ServiceRegistry
    {
        public NodeBridgeInstaller()
        {
            this.AddSingleton<INodeBridge, NodeBridge.NodeBridge>();
        }
    }
}