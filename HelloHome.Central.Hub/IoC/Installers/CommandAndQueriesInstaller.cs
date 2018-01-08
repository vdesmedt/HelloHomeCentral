using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using HelloHome.Central.Hub.Commands;
using HelloHome.Central.Hub.Handlers;
using HelloHome.Central.Hub.Queries;

namespace HelloHome.Central.Hub.IoC.Installers
{
    public class CommandAndQueriesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            //Queries & Commands
            container.Register(Classes.FromAssemblyContaining<IQuery>()
                .BasedOn<IQuery>()
                .WithServiceAllInterfaces()
                .LifestyleBoundTo<IMessageHandler>());
            container.Register(Classes.FromAssemblyContaining<ICommand>()
                .BasedOn<ICommand>()
                .WithServiceAllInterfaces()
                .LifestyleBoundTo<IMessageHandler>());
        }
    }
}