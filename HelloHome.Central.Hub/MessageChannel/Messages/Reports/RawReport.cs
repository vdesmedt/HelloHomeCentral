using System.Text;

namespace HelloHome.Central.Hub.MessageChannel.Messages.Reports
{
	public class RawReport : IncomingMessage
	{

		public byte[] Bytes { get; set; }

		public RawReport (byte[] rawMessage)
		{
			Bytes = rawMessage;			
		}

		public override string ToString ()
		{
			var sb = new StringBuilder (Bytes.Length * 2);
			foreach (var b in Bytes)
				sb.Append(b.ToString("X2"));
			return $"[RawMessage: Bytes={sb}]";
		}
	}
}

