using System.Runtime.Serialization;

namespace HelloHome.Central.Hub.MessageChannel.Messages.Reports
{
    public class NodeStartedReport : IncomingMessage, ISignedMessage
	{
		public string Version { get; set; }
		public long Signature { get; set; }

		public override string ToString ()
		{
			return $"[NodeStartedReport: From={FromRfAddress}, Version={Version}, Signature={Signature}]";
		}
	}
}