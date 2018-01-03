using System.Linq;
using System.Text;
using HelloHome.Central.Hub.MessageChannel.Messages;
using HelloHome.Central.Hub.MessageChannel.Messages.Reports;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers
{
	public class CommentParser : IMessageParser
	{
		private readonly Encoding _encoding = Encoding.ASCII;

		#region IMessageParser implementation

	    public IncomingMessage Parse (byte[] record)
		{
			return new CommentReport (_encoding.GetString (record.Skip(2).ToArray()));
		}
		#endregion
	}
}

