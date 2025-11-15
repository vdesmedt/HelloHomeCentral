using System;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using HelloHome.Central.Common.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel
{
    public class SerialByteStream(ILogger<SerialByteStream> logger, IOptionsMonitor<SerialConfig> config) : IByteStream, IDisposable
    {
        private readonly SerialPort _port = new(config.CurrentValue.Port, config.CurrentValue.BaudRate, Parity.None, 8, StopBits.One)
        {
            ReadTimeout = config.CurrentValue.TimeOut
        };

        public void Open()
        {
            logger.LogInformation("Opening serial port {port-name} with baudRate {baud-rate} and timeout {timeout} (8N1)",
                _port.PortName, _port.BaudRate, _port.ReadTimeout);
            _port.Open();
            logger.LogInformation("Waiting 2000ms for RFM2Pi node to be ready");
            Thread.Sleep(2000);
            logger.LogInformation("Port {port-name} opened & ready", _port.PortName);
        }

        public int ByteAvailable()
        {
            return _port.BytesToRead;
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            try
            {
                if (!_port.IsOpen)
                    _port.Open();
                return _port.Read(buffer, offset, count);
            }
            catch (TimeoutException)
            {
                return 0;
            }
        }

        public async Task<int> ReadAsync(byte[] buffer, int offset, int count)
        {
            try
            {
                if (!_port.IsOpen)
                    _port.Open();
                return await _port.BaseStream.ReadAsync(buffer, offset, count);
            }
            catch (TimeoutException)
            {
                return 0;
            }
        }

        public string ReadLine()
        {
            try
            {
                return _port.ReadLine();
            }
            catch (TimeoutException)
            {
                return String.Empty;
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