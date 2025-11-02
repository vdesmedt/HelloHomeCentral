using HelloHome.Central.Domain.Logic;
using HelloHome.Central.Domain.Logic.CoreLogic;
using HelloHome.Central.Domain.Logic.RfAddressStrategy;
using HelloHome.Central.Domain.Messages.Commands;
using Lamar;

namespace HelloHome.Central.Common.IoC.Registries
{
    public class BusinessLogicRegistry : ServiceRegistry 
    {
        public BusinessLogicRegistry()
        {
            For<ITimeProvider>().Use<TimeProvider>().Singleton();
            For<IRfAddressStrategy>().Use<FillHolesRfAddressStrategy>().Scoped();
            For<INodeLogger>().Use<NodeLogger>().Singleton();
            For<ICoreLogic>().Use<CoreLogic>().Scoped();
            For<IActionToCommandMapper>().Use<ActionToCommandMapper>().Singleton();
        }
    }
}