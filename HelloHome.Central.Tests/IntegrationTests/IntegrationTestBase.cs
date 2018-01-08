using Castle.Windsor;
using Castle.MicroKernel.Registration;
using HelloHome.Central.Domain;
using HelloHome.Central.Hub;
using HelloHome.Central.Hub.IoC.Installers;
using HelloHome.Central.Hub.MessageChannel;
using HelloHome.Central.Repository;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace HelloHome.Central.Tests.IntegrationTests
{
    public abstract class IntegrationTestBase
    {
        private readonly WindsorContainer _windsorContainer;
        protected HhDbContext DbCtx;

        protected IntegrationTestBase()
        {
            _windsorContainer = new WindsorContainer();
            _windsorContainer.Install(
                new FacilityInstaller(),
                new HandlerInstaller(),
                new CommandAndQueriesInstaller(),
                new HubInstaller()
            );
                        
            _windsorContainer.Register(Component.For<IMessageChannel>().Instance(MsgChannelMoq.Object));
            Hub = _windsorContainer.Resolve<MessageHub>();
        }

        protected void RegisterDbContext(string inMemoryDbName)
        {
            var options = new DbContextOptionsBuilder<HhDbContext>()
                .UseInMemoryDatabase(databaseName: inMemoryDbName)
                .Options;
            DbCtx = new HhDbContext(options);
            _windsorContainer.Register(Component.For<IUnitOfWork>().Instance(DbCtx));
        }

        public Mock<TMock> RegisterMock<TMock>() where TMock : class
        {
            var mock = new Mock<TMock>();
            _windsorContainer.Register(Component.For<TMock>().Instance(mock.Object));
            return mock;
        }

        public Mock<IMessageChannel> MsgChannelMoq { get; } = new Mock<IMessageChannel>();
        public MessageHub Hub { get; private set; }
    }
    
}