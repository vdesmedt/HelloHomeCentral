using Castle.Windsor;
using Castle.MicroKernel.Registration;
using HelloHome.Central.Domain;
using HelloHome.Central.Hub;
using HelloHome.Central.Hub.IoC.Installers;
using HelloHome.Central.Hub.MessageChannel;
using HelloHome.Central.Hub.Queries;
using Moq;

namespace HelloHome.Central.Tests.IntegrationTests
{
    public class IntegrationTestFixture
    {
        private WindsorContainer _windsorContainer;

        public IntegrationTestFixture()
        {
            _windsorContainer = new WindsorContainer();
            _windsorContainer.Install(
                new FacilityInstaller(),
                new HandlerInstaller(),
                new HubInstaller()
            );
            
            _windsorContainer.Register(Component.For<IUnitOfWork>().Instance(UnitOfWorkMoq.Object));
            _windsorContainer.Register(Component.For<IMessageChannel>().Instance(MsgChannelMoq.Object));
            Hub = _windsorContainer.Resolve<MessageHub>();
        }

        public Mock<TMock> RegisterMock<TMock>() where TMock : class
        {
            var mock = new Mock<TMock>();
            _windsorContainer.Register(Component.For<TMock>().Instance(mock.Object));
            return mock;
        }

        public Mock<IMessageChannel> MsgChannelMoq { get; } = new Mock<IMessageChannel>();
        public Mock<IUnitOfWork> UnitOfWorkMoq { get; } = new Mock<IUnitOfWork>();
        public MessageHub Hub { get; private set; }
    }
    
}