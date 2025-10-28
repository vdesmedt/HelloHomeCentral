
using HelloHome.Central.Domain.Messages;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Encoders.Base
{
	
	public interface IMessageEncoder
	{
		byte[] Encode(OutgoingMessage message);
	}
}
