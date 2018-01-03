using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using HelloHome.Central.Domain;
using HelloHome.Central.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HelloHome.Central.Hub.IoC.Installers
{
    public class DbContextInstaller : IWindsorInstaller
    {
        private readonly IConfigurationRoot _config;

        public DbContextInstaller(IConfigurationRoot config)
        {
            _config = config;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var dbCOntextOptionsBuilder = new DbContextOptionsBuilder<HelloHomeContext>();

            var loggerFactory = new LoggerFactory();
            loggerFactory.AddConsole();

            dbCOntextOptionsBuilder
                .UseMySql(_config.GetConnectionString("Default"))
                .UseLoggerFactory(loggerFactory);

            container.Register(
                Component.For<IUnitOfWork>()
                    .ImplementedBy<HelloHomeContext>()
                    .LifestyleSingleton(),
                Component.For<DbContextOptions<HelloHomeContext>>()
                    .Instance(dbCOntextOptionsBuilder.Options)
            );
        }
    }
}