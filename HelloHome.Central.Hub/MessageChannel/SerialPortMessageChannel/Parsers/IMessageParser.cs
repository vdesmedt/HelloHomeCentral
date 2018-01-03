
using HelloHome.Central.Hub.MessageChannel.Messages;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers
{
	public interface IMessageParser
	{
	    IncomingMessage Parse(byte[] record);
	}
}

