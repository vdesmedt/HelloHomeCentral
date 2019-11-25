using Lamar;

namespace HelloHome.Central.Hub.IoC.Installers
{
    public class HubInstaller : ServiceRegistry
    {
        public HubInstaller()
        {
            ForConcreteType<MessageHub>();
        }
    }
}