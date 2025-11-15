using HelloHome.Central.Common.IoC.Registries;
using Lamar;


namespace HelloHome.Central.Hub.IoC.Registries
{
    public class HubServiceRegistry : ServiceRegistry
    {
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