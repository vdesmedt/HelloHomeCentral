
using HelloHome.Central.Hub.MessageChannel.Messages;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Encoders.Base
{
	
	public interface IMessageEncoder
	{
		byte[] Encode(OutgoingMessage message);
	}
}
