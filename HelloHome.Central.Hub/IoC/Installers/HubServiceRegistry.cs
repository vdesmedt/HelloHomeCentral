using System.Diagnostics;
using Lamar;
using NLog;

namespace HelloHome.Central.Hub.IoC.Installers
{
    public class HubServiceRegistry : ServiceRegistry
    {
        private static readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();		
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