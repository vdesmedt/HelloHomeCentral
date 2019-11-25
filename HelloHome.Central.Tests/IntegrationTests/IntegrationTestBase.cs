using System;
using System.Diagnostics;
using HelloHome.Central.Domain;
using HelloHome.Central.Hub;
using HelloHome.Central.Hub.IoC.Installers;
using HelloHome.Central.Hub.MessageChannel;
using HelloHome.Central.Repository;
using Lamar;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace HelloHome.Central.Tests.IntegrationTests
{
    public abstract class IntegrationTestBase
    {
        protected readonly Container _container;

        protected IntegrationTestBase()
        {
            _container = Container.For<HubServiceRegistry>();

            _container.Configure(c =>
            {
                c.AddSingleton<IMessageChannel>(p => MsgChannelMoq.Object);
            });
        }

        protected HhDbContext RegisterDbContext(string inMemoryDbName)
        {
            var options = new DbContextOptionsBuilder<HhDbContext>()
                .UseInMemoryDatabase(databaseName: inMemoryDbName)
                .Options;
            var dbCtx = new HhDbContext(options);
            _container.Configure(c =>
            {
                c.Replace(ServiceDescriptor.Singleton<IUnitOfWork>(dbCtx));
            });
            return dbCtx;
        }

        protected Mock<TMock> RegisterMock<TMock>() where TMock : class
        {
            var mock = new Mock<TMock>();
            _container.Configure(c => { c.AddSingleton<TMock>(mock.Object); });
            return mock;
        }

        private Mock<IMessageChannel> MsgChannelMoq { get; } = new Mock<IMessageChannel>();
        
        private MessageHub _hub;
        protected MessageHub Hub => _hub ??= _container.GetInstance<MessageHub>();
    }
}