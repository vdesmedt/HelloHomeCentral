using System;
using System.Threading;
using System.Threading.Tasks;
using HelloHome.Central.Common.Configuration;
using NLog;
using RJCP.IO.Ports;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel
{
    public class SerialByteStream : IByteStream, IDisposable
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly SerialPortStream _port;

        public SerialByteStream(SerialConfig config)
        {
            Logger.Info(() => $"Opening serial port {config.Port} with baudRate {config.BaudRate} (8N1)");
            _port = new SerialPortStream(config.Port, config.BaudRate, 8, Parity.None, StopBits.One)
            {
                ReadTimeout = config.TimeOut
            };
            _port.Open();
            Logger.Info(() => $"Serial port {config.Port} opened");
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