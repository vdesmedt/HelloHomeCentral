using HelloHome.Central.Common;
using HelloHome.Central.Domain.Logic;
using HelloHome.Central.Domain.Logic.CoreLogic;
using HelloHome.Central.Domain.Logic.RfAddressStrategy;
using HelloHome.Central.Hub.MessageChannel.Messages.Commands;
using Lamar;

namespace HelloHome.Central.Hub.IoC.Installers
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