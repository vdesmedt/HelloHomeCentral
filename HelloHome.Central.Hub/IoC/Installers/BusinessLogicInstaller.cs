using HelloHome.Central.Common;
using HelloHome.Central.Hub.Commands;
using HelloHome.Central.Hub.Logic;
using HelloHome.Central.Hub.Logic.RfAddressStrategy;
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
        }
    }
}