using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using HelloHome.Central.Common;
using HelloHome.Central.Hub.Commands;
using HelloHome.Central.Hub.Handlers;
using HelloHome.Central.Hub.Logic;
using HelloHome.Central.Hub.Logic.RfAddressStrategy;

namespace HelloHome.Central.Hub.IoC.Installers
{
    public class BusinessLogicInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<ITimeProviderFactory>()
                    .AsFactory(),
                Component.For<ITimeProvider>()
                    .ImplementedBy<TimeProvider>(),
                Component.For<IRfAddressStrategy>()
                    .ImplementedBy<FillHolesRfAddressStrategy>()
                    .LifestyleBoundTo<IMessageHandler>(),
                Component.For<ITouchNode>().ImplementedBy<TouchNode>()
            );

            NodeLogger.TimeProviderFactory = container.Resolve<ITimeProviderFactory>();
        }
    }
}