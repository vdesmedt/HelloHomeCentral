using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using HelloHome.Central.Domain;
using HelloHome.Central.Hub.Handlers;
using HelloHome.Central.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HelloHome.Central.Hub.IoC.Installers
{
    public class DbContextInstaller : IWindsorInstaller
    {
        private readonly string _connectionString;

        public DbContextInstaller(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var dbCOntextOptionsBuilder = new DbContextOptionsBuilder<HelloHomeContext>();

            var loggerFactory = new LoggerFactory();
            loggerFactory.AddConsole();

            dbCOntextOptionsBuilder
                .UseMySql(_connectionString)
                .UseLoggerFactory(loggerFactory);

            container.Register(
                Component.For<IUnitOfWork>()
                    .ImplementedBy<HelloHomeContext>()
                    .LifestyleBoundTo<IMessageHandler>(),
                Component.For<DbContextOptions<HelloHomeContext>>()
                    .Instance(dbCOntextOptionsBuilder.Options)
            );
        }
    }
}