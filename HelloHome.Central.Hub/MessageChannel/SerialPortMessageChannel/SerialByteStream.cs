using System;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using HelloHome.Central.Common.Configuration;
using Microsoft.Extensions.Options;
using NLog;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel
{
    public class SerialByteStream : IByteStream, IDisposable
    {
        private static readonly Logger Logger = LogManager.GetLogger(nameof(SerialByteStream));
        private readonly SerialPort _port;

        public SerialByteStream(IOptionsMonitor<SerialConfig> config)
        {
            _port = new SerialPort(config.CurrentValue.Port, config.CurrentValue.BaudRate, Parity.None, 8, StopBits.One)
            {
                ReadTimeout = config.CurrentValue.TimeOut
            };
        }

        public void Open()
        {
            Logger.Info("Opening serial port {portname} with baudRate {baudrate} and timeout {timeout} (8N1)", _port.PortName, _port.BaudRate, _port.ReadTimeout);
            _port.Open();
            Logger.Info(() => $"Port {_port.PortName} opened");
        }

        public int ByteAvailable()
        {
            return _port.BytesToRead;
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            try
            {
                if(!_port.IsOpen)
                    _port.Open();
                return _port.Read(buffer, offset, count);
            }
            catch (TimeoutException)
            {
                return 0;
            }
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