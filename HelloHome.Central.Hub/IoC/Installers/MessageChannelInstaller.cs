using System.Linq;
using HelloHome.Central.Common.IoC;
using HelloHome.Central.Hub.Handlers.Base;
using HelloHome.Central.Hub.IoC.Factories;
using HelloHome.Central.Hub.MessageChannel;
using HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel;
using HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Encoders;
using HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Encoders.Base;
using HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers;
using HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers.Base;
using Lamar;
using Microsoft.Extensions.Options;

namespace HelloHome.Central.Hub.IoC.Installers
{
    public class MessageChannelInstaller : ServiceRegistry
    {
        public MessageChannelInstaller()
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