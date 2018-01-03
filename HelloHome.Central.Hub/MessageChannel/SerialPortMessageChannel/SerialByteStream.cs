using Microsoft.Extensions.Configuration;
using RJCP.IO.Ports;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel
{
    public class SerialByteStream : IByteStream
    {
        private SerialPortStream _port;

        public SerialByteStream(IConfigurationRoot config)
        {
            _port = new SerialPortStream(config["messageChannel:serial:port"],
                config.GetValue<int>("messageChannel:seria:baud"), 8, Parity.None, StopBits.One);
            _port.Open();            
        }
    }
}