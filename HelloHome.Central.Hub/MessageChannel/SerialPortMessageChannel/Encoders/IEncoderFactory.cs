using HelloHome.Central.Hub.MessageChannel.Messages;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Encoders
{
    public interface IEncoderFactory
    {
        IMessageEncoder Create<T>(T outgoingMessage) where T : OutgoingMessage;
        void Release(IMessageEncoder encoder);
    }
}