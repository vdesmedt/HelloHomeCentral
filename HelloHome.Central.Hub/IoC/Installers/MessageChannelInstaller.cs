using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using HelloHome.Central.Hub.IoC.FactoryComponentSelector;
using HelloHome.Central.Hub.MessageChannel;
using HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel;
using HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Encoders;
using HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers;

namespace HelloHome.Central.Hub.IoC.Installers
{
    public class MessageChannelInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IMessageChannel>()
                    .ImplementedBy<SerialPortMessageChannel>()
                    .LifestyleSingleton(),
                Component.For<IByteStream>()
                    .ImplementedBy<SerialByteStream>()
                    .LifestyleSingleton()
            );
            
            //Parsers
            container.Register(
                Component.For<MessageParserComponentSelector>(),
                Component.For<IMessageParserFactory>()
                    .AsFactory(typeof(MessageParserComponentSelector)),
                Classes.FromAssemblyContaining<IMessageParser>()
                    .BasedOn<IMessageParser>()
                    .WithServiceSelf()
                    .Configure(x => x.LifestyleSingleton())
            );
            
            //Encoders
            container.Register(
                Component.For<EncoderFactoryComponentSelector>(),
                Component.For<IMessageEncoderFactory>()
                    .AsFactory(typeof(EncoderFactoryComponentSelector)),
                Classes.FromAssemblyContaining<IMessageEncoder>()
                    .BasedOn<IMessageEncoder>()
                    .WithServiceSelf()
                    .Configure(x => x.LifestyleSingleton()),
                Component.For<PinConfigEncoder>()
            );
        }
    }
}