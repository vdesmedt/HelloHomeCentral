
using HelloHome.Central.Hub.MessageChannel.Messages;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers.Base
{
	public interface IMessageParser
	{
	    IncomingMessage Parse(byte[] record);
	}
}

