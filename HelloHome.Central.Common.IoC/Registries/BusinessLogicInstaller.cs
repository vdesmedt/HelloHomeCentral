using HelloHome.Central.Domain.Logic;
using HelloHome.Central.Domain.Logic.CoreLogic;
using HelloHome.Central.Domain.Logic.RfAddressStrategy;
using HelloHome.Central.Domain.Messages.Commands;
using Lamar;

namespace HelloHome.Central.Common.IoC.Registries
{
    public class BusinessLogicInstaller : ServiceRegistry 
    {
        public BusinessLogicInstaller()
        {
            For<ITimeProvider>().Use<TimeProvider>().Singleton();
            For<IRfAddressStrategy>().Use<FillHolesRfAddressStrategy>().Scoped();
            For<INodeLogger>().Use<NodeLogger>().Singleton();
            For<ICoreLogic>().Use<CoreLogic>().Scoped();
            For<IActionToCommandMapper>().Use<ActionToCommandMapper>().Singleton();
        }
    }
}