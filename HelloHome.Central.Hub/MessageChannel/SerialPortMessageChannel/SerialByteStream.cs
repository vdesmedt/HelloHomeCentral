using HelloHome.Central.Common.Configuration;
using Microsoft.Extensions.Configuration;
using RJCP.IO.Ports;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel
{
    public class SerialByteStream : IByteStream
    {
        private SerialPortStream _port;

        public SerialByteStream(SerialConfig config)
        {
            _port = new SerialPortStream(config.Port,
                config.BaudRate, 8, Parity.None, StopBits.One);
            _port.Open();            
        }
    }
}