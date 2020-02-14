using System.Linq;
using HelloHome.Central.Common.IoC;
using HelloHome.Central.Hub.Handlers.Base;
using HelloHome.Central.Hub.IoC.Factories;
using Lamar;


namespace HelloHome.Central.Hub.IoC.Installers
{
    public class HandlerInstaller : ServiceRegistry
    {
        public HandlerInstaller()
        {
            For<IMessageHandlerFactory>().Use<MessageHandlerFactory>().Ctor<Container>().Is(c => (Container)c).Singleton();
            Scan(scanner =>
            {
                scanner.AssemblyContainingType<IMessageHandler>();
                scanner.Include(_ => _.GetInterfaces().Contains(typeof(IMessageHandler)));
                scanner.Convention<WithAllInterfacesRegistrationConvention>();
            });
        }
    }
}