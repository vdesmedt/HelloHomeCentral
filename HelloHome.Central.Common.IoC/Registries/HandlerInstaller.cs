using HelloHome.Central.Common.IoC.Factories;
using HelloHome.Central.Domain.Handlers.Base;
using Lamar;

namespace HelloHome.Central.Common.IoC.Registries
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