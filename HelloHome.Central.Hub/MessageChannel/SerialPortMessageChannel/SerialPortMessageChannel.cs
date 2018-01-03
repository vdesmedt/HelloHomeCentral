using System.Threading;
using HelloHome.Central.Hub.MessageChannel.Messages;
using Microsoft.Extensions.Configuration;
using RJCP.IO.Ports;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel
{
    public class SerialPortMessageChannel : IMessageChannel
    {
        private SerialPortStream _port { get; set; }

        public SerialPortMessageChannel(IConfigurationRoot config)
        {
            _port = new SerialPortStream(config["messageChannel:serial:port"],
                config.GetValue<int>("messageChannel:seria:baud"), 8, Parity.None, StopBits.One);
            _port.Open();
        }


        public IncomingMessage TryReadNext(int millisecond, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public void Send(OutgoingMessage message)
        {
            throw new System.NotImplementedException();
        }
    }
}