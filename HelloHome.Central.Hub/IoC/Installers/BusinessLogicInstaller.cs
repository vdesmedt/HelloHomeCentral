using HelloHome.Central.Common;
using HelloHome.Central.Domain.Logic;
using HelloHome.Central.Domain.Logic.RfAddressStrategy;
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