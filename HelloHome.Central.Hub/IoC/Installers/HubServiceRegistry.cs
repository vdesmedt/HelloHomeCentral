using Lamar;

namespace HelloHome.Central.Hub.IoC.Installers
{
    public class HubServiceRegistry : ServiceRegistry
    {
        public HubServiceRegistry()
        {
            IncludeRegistry<ConfigInstaller>();
            IncludeRegistry<BusinessLogicInstaller>();
            IncludeRegistry<CommandAndQueriesInstaller>();
            IncludeRegistry<DbContextInstaller>();
            IncludeRegistry<HandlerInstaller>();
            IncludeRegistry<HubInstaller>();
            IncludeRegistry<MessageChannelInstaller>();
        }
    }
}