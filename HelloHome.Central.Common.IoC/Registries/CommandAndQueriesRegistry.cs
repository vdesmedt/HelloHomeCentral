using HelloHome.Central.Domain.CmdQrys.Base;
using Lamar;

namespace HelloHome.Central.Common.IoC.Registries
{
    public partial class CommandAndQueriesRegistry : ServiceRegistry
    {
        public CommandAndQueriesRegistry()
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