using System.Threading;
using System.Threading.Tasks;
using HelloHome.Central.Hub.MessageChannel.Messages;

namespace HelloHome.Central.Hub.MessageChannel
{
    public interface IMessageChannel
    {
        IncomingMessage TryReadNext();
        void Send(OutgoingMessage message);
        void Close();
    }
}