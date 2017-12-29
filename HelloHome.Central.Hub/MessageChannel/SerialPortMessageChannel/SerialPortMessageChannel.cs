using System.Threading;
using HelloHome.Central.Hub.MessageChannel.Messages;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel
{
    public class SerialPortMessageChannel : IMessageChannel
    {
        public IncomingMessage TryReadNext(int millisecond, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public void Send(OutgoingMessage message)
        {
            throw new System.NotImplementedException();
        }
    }
}