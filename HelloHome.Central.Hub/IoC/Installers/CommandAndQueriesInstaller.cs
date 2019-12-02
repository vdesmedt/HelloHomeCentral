using HelloHome.Central.Hub.CmdQrys.Base;
using HelloHome.Central.Hub.Commands;
using Lamar;

namespace HelloHome.Central.Hub.IoC.Installers
{
    public partial class CommandAndQueriesInstaller : ServiceRegistry
    {
        public CommandAndQueriesInstaller()
        {
            Scan(scanner =>
            {
                scanner.AssemblyContainingType<IQuery>();
                scanner.Include(_ => typeof(IQuery).IsAssignableFrom(_));
                scanner.Include(_ => typeof(ICommand).IsAssignableFrom(_));
                scanner.Convention<WithAllInterfacesRegistrationConvention>();
            });
        }
    }
}