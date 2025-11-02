using HelloHome.Central.Common.IoC.Registries;
using Lamar;
using NLog;

namespace HelloHome.Central.Hub.IoC.Registries
{
    public class HubServiceRegistry : ServiceRegistry
    {
        private static readonly Logger Logger = NLog.LogManager.GetLogger(nameof(HubServiceRegistry));		
        public HubServiceRegistry()
        {
            IncludeRegistry<BusinessLogicRegistry>();
            IncludeRegistry<CommandAndQueriesRegistry>();
            IncludeRegistry<DbContextRegistry>();
            IncludeRegistry<HandlerRegistry>();
            IncludeRegistry<NodeBridgeRegistry>();
            IncludeRegistry<MessageChannelRegistry>();
        }
    }
}