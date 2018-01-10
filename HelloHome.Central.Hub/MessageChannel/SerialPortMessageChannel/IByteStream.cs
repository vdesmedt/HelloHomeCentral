using System.Threading;
using System.Threading.Tasks;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel
{
    public interface IByteStream
    {
        Task<int> ReadAsync(byte[] buffer, int offset, int cout, CancellationToken cToken);
        Task WriteAsync(byte[] buffer, int offset, int cout, CancellationToken cToken);        
    }
}