using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using HelloHome.Central.Common.Configuration;

namespace HelloHome.Central.Hub.IoC.Installers
{
    public class ConfigInstaller : IWindsorInstaller
    {
        private readonly HhConfig _config;

        public ConfigInstaller(HhConfig config)
        {
            _config = config;
        }
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<HhConfig>().Instance(_config),
                Component.For<SerialConfig>().Instance(_config.SerialConfig),
                Component.For<EmonCmsConfig>().Instance(_config.EmonCmsConfig)
            );
        }
    }
}