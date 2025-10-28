
using HelloHome.Central.Domain.Messages;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers.Base
{
	public interface IMessageParser
	{
	    IncomingMessage Parse(byte[] record);
	}
}

