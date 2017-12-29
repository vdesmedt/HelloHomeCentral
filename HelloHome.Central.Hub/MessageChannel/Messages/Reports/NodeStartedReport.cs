using System.Runtime.Serialization;

namespace HelloHome.Central.Hub.MessageChannel.Messages.Reports
{
    public class NodeStartedReport : Report, ISignedMessage
	{
	    public int Major { get; set; }
	    public int Minor { get; set; }
	    public int OldSignature  { get; set; }
	    public long Signature { get; set; }
	    public bool NeedNewRfAddress { get; set; }

		public override string ToString ()
		{
			return $"[NodeStartedReport: From={FromRfAddress}, Major={Major}, Minor={Minor}, OldSignature={OldSignature}, Signature={Signature}, NeedNewRfAddress={NeedNewRfAddress}]";
		}
	}
}