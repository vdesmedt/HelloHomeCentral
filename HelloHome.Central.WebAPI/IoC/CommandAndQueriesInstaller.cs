using HelloHome.Central.Common.IoC;
using HelloHome.Central.Domain.CmdQrys.Base;
using Lamar;

namespace HelloHome.Central.WebAPI.IoC
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