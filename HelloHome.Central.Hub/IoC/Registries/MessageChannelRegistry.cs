using System.Linq;
using HelloHome.Central.Common.IoC;
using HelloHome.Central.Domain.Handlers.Base;
using HelloHome.Central.Hub.IoC.Factories;
using HelloHome.Central.Hub.MessageChannel;
using HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel;
using HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Encoders.Base;
using HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers.Base;
using Lamar;

namespace HelloHome.Central.Hub.IoC.Registries
{
    public class MessageChannelRegistry : ServiceRegistry
    {
        public MessageChannelRegistry()
        {
            For<IMessageChannel>().Use<SerialPortMessageChannel>().Singleton();
            For<IByteStream>().Use<SerialByteStream>().Singleton();
            For<IMessageParserFactory>().Use<MessageParserFactory>().Ctor<Container>().Is(c => (Container) c);
            For<IMessageEncoderFactory>().Use<MessageEncoderFactory>().Ctor<Container>().Is(c => (Container) c);
            Scan(scanner =>
            {
                scanner.AssemblyContainingType<IMessageParser>();
                scanner.Include(_ => _.GetInterfaces().Contains(typeof(IMessageEncoder)));
                scanner.Include(_ => _.GetInterfaces().Contains(typeof(IMessageParser)));
                scanner.Include(_ => _.GetInterfaces().Contains(typeof(IMessageHandler)));
                scanner.Convention<WithAllInterfacesRegistrationConvention>();
            });
        }
    }
}