using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using HelloHome.Central.Common.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using IConfiguration = Castle.Core.Configuration.IConfiguration;

namespace HelloHome.Central.Hub.IoC.Installers
{
    public class ConfigInstaller : IWindsorInstaller    
    {
        private readonly IConfiguration _configuration;

        public ConfigInstaller(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var serialConfig = 
            container.Register(Component.For<SerialConfig>().Instance(new SerialConfig()));
        }
    }
}