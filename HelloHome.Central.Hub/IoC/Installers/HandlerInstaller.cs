﻿using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using HelloHome.Central.Common;
using HelloHome.Central.Hub.Commands;
using HelloHome.Central.Hub.Handlers;
using HelloHome.Central.Hub.Handlers.Factory;
using HelloHome.Central.Hub.IoC.FactoryComponentSelector;
using HelloHome.Central.Hub.Logic.RfAddressStrategy;
using HelloHome.Central.Hub.Queries;

namespace HelloHome.Central.Hub.IoC.Installers
{
    public class HandlerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            //MessageHandlers
            container.Register(
                Classes.FromAssemblyContaining<IMessageHandler>()
                    .BasedOn(typeof(MessageHandler<>))
                    .WithServiceSelf()
                    .LifestyleTransient(),
                Component.For<MessageHandlerComponentSelector>()
                    .LifestyleSingleton(),
                Component.For<IMessageHandlerFactory>()
                    .AsFactory(typeof(MessageHandlerComponentSelector))
            );

            //Logic
            container.Register(
                Component.For<ITimeProvider>()
                    .ImplementedBy<TimeProvider>(),
                Component.For<IRfAddressStrategy>()
                    .ImplementedBy<FillHolesRfAddressStrategy>()
                    .LifestyleBoundTo<IMessageHandler>(),
                Component.For<ITouchNode>().ImplementedBy<TouchNode>()
            );
            
            TimeProvider.Current = container.Resolve<ITimeProvider>();
        }
    }
}