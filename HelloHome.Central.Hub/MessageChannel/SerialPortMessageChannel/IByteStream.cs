using System.Threading;
using System.Threading.Tasks;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel
{
    public interface IByteStream
    {
        void Write(byte[] buffer, int offset, int cout);
        int Read(byte[] buffer, int offset, int cout);
        void Close();
        void Open();
        int ByteAvailable();
    }
}