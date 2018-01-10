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
        
        public async Task<int> ReadAsync(byte[] buffer, int offset, int cout, CancellationToken cToken)
        {
            try
            {
                return await _port.ReadAsync(buffer, offset, cout, cToken);

            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public async Task WriteAsync(byte[] buffer, int offset, int cout, CancellationToken cToken)
        {
            await _port.WriteAsync(buffer, offset, cout, cToken);
        }


        public void Dispose()
        {
            if (_port.IsOpen)
                _port.Close();
            _port.Dispose();
        }        
    }
}