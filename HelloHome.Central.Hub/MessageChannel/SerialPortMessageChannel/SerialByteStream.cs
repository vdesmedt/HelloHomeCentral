using System;
using System.Threading;
using System.Threading.Tasks;
using HelloHome.Central.Common.Configuration;
using RJCP.IO.Ports;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel
{
    public class SerialByteStream : IByteStream, IDisposable
    {
        private readonly SerialPortStream _port;

        public SerialByteStream(SerialConfig config)
        {
            _port = new SerialPortStream(config.Port, config.BaudRate, 8, Parity.None, StopBits.One)
            {
                ReadTimeout = config.TimeOut
            };
            _port.Open();            
        }

        public int Read(byte[] buffer, int offset, int cout)
        {
            return _port.Read(buffer, offset, cout);
        }


        public void Write(byte[] buffer, int offset, int cout)
        {
            _port.Write(buffer, offset, cout);
        }

        public void Close()
        {
            _port.Close();
        }

        public void Dispose()
        {
            if (_port.IsOpen)
                _port.Close();
            _port.Dispose();
        }        
    }
}