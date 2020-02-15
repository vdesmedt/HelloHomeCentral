using System.Threading;
using System.Threading.Tasks;
using HelloHome.Central.Hub.MessageChannel.Messages;

namespace HelloHome.Central.Hub.MessageChannel
{
    public interface IMessageChannel
    {
        void Open();
        /// <summary>
        /// Returns a IncomingMessage if found completed in UART within the timeout
        /// </summary>
        /// <returns></returns>
        IncomingMessage TryReadNext();
        void Send(OutgoingMessage message);
        void Close();
    }
}