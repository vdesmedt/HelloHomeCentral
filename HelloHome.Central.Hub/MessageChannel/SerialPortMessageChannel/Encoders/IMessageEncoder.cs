
using HelloHome.Central.Hub.MessageChannel.Messages;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Encoders
{
	
	public interface IMessageEncoder
	{
	    byte[] Encode(Message message);
	}
}
