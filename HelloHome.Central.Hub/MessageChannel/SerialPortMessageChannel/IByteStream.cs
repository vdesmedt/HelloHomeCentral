using System.Threading;
using System.Threading.Tasks;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel
{
    public interface IByteStream 
    {
        void Write(byte[] buffer, int offset, int count);
        int Read(byte[] buffer, int offset, int count);
        Task<int> ReadAsync(byte[] buffer, int offset, int count);
        void Close();
        void Open();
        int ByteAvailable();
        string ReadLine();
    }
}