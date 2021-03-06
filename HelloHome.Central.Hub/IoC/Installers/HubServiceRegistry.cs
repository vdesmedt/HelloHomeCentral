using System.Diagnostics;
using Lamar;
using NLog;

namespace HelloHome.Central.Hub.IoC.Installers
{
    public class HubServiceRegistry : ServiceRegistry
    {
        private static readonly Logger Logger = NLog.LogManager.GetLogger(nameof(HubServiceRegistry));		
        public HubServiceRegistry()
        {
            IncludeRegistry<BusinessLogicInstaller>();
            IncludeRegistry<CommandAndQueriesInstaller>();
            IncludeRegistry<DbContextInstaller>();
            IncludeRegistry<HandlerInstaller>();
            IncludeRegistry<NodeBridgeInstaller>();
            IncludeRegistry<MessageChannelInstaller>();
        }
    }
}