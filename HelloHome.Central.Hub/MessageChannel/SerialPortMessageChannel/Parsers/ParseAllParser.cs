using HelloHome.Central.Domain.Messages;
using HelloHome.Central.Domain.Messages.Reports;
using HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers.Base;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers
{
	[NonDiscriminatedParser]
	public class ParseAllParser : IMessageParser
	{
		#region IMessageParser implementation

	    public IncomingMessage Parse (byte[] record)
		{
			return new RawReport (record);
		}
		#endregion
	}
}

